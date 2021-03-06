#!/usr/bin/env python
# coding=utf-8
import os
import sys
import json
import ast
import stat
import sqlite3
import hashlib
import threading
import time
import qcloud_cos
import threadpool
import logging
import logging.handlers
from qcloud_cos import CosClient
from qcloud_cos import CosConfig
from qcloud_cos import UploadFileRequest
from qcloud_cos import StatFileRequest
from qcloud_cos import StatFolderRequest
from qcloud_cos import DelFileRequest
from qcloud_cos import DelFolderRequest
from qcloud_cos import CreateFolderRequest

################################################################################
# 日志配置                                                                     #
################################################################################
class CosSyncLog(object):
    @classmethod
    def set_log(cls, level=logging.INFO, log_name='./log/cos_sync.log'):
        # 定义一个RotatingFileHandler
        # 最多备份20个日志文件，每个日志文件最大100MB
        logger = logging.getLogger('tencent_cos_sync')
        Rthandler = logging.handlers.RotatingFileHandler(log_name,
                maxBytes=100*1024*1024, backupCount=20)
        Rthandler.setLevel(level)
        formatter = logging.Formatter('''%(asctime)s %(filename)s
        [line:%(lineno)d] %(levelname)s %(message)s''')
        Rthandler.setFormatter(formatter)
        logger.addHandler(Rthandler)
        logger.setLevel(logging.INFO)
        return logger

# 生成全局的CosSyncLog对象
cos_sync_logger = CosSyncLog.set_log()

################################################################################
# cos_sync工具的配置类, 从配置文件读取配置, 并校验配置是否正确                 #
################################################################################
class CosSyncConfig(object):

    def __init__(self, config_path=u'./conf/cos_sync_config.json'):
        # 判断文件是否存在
        if (not os.path.isfile(config_path)):
            self.init_config_err = u'config file %s not exist' % (config_path)
            self.init_config_flag = False
            return

        # 读取配置
        config_file = open(config_path)
        try:
            config_str = config_file.read()
            self.config_json = json.loads(config_str)
        except:
            self.init_config_err = u'config file is not valid json'
            self.init_config_flag = False
            return
        finally:
            config_file.close()

        # 判断下文件的编码, 配置文件必须以UTF-8编码

        # 检查是否包含了必须的配置
        valid_config_key_arr = [u'appid', u'secret_id', u'secret_key', u'bucket',
                u'timeout', u'thread_num', u'local_path', u'cos_path',
                u'delete_sync', u'daemon_mode', u'daemon_interval', u'enable_https']

        for config_key in valid_config_key_arr:
            if (config_key not in self.config_json.keys()):
                self.init_config_err = u'config file not contain %s' % (config_key)
                self.init_config_flag = False
                return
            config_value = self.config_json[config_key]
            if not isinstance(config_value, unicode):
                self.init_config_err = u'config %s value must be unicode str' % (config_key)
                self.init_config_flag = False
                return

        # 检查appid
        try:
            int(self.config_json[u'appid'])
        except:
            self.init_config_err = u'appid is illegal'
            self.init_config_flag = False
            return

        # 只能同步目录
        if not os.path.isdir(self.config_json[u'local_path']):
            self.init_config_err = u'local_path is not effective dir path'
            self.init_config_flag = False
            return

        # 如果同步路径是相对路径,则转为绝对路径
        self.config_json[u'local_path'] = os.path.abspath(self.config_json[u'local_path'])
        self.config_json[u'local_path'] = self.config_json[u'local_path'].replace(u'\\', u'/')

        # 检查cos路径是否以'/'开始
        if self.config_json[u'cos_path'][0] != u'/':
            self.init_config_err = u'cos_path must start with bucket root /'
            self.init_config_flag = False
            return

        # 检查timeout
        try:
            int(self.config_json[u'timeout'])
        except:
            self.init_config_err = u'timeout is illegal'
            self.init_config_flag = False
            return

        # 检查thread_num
        try:
            int(self.config_json[u'thread_num'])
        except:
            self.init_config_err = u'thread_num is illegal'
            self.init_config_flag = False
            return

        # 检查delete_sync
        try:
            int(self.config_json[u'delete_sync'])
        except:
            self.init_config_err = u'delete_sync is illegal'
            self.init_config_flag = False
            return

        # 检查daemon_mode
        try:
            int(self.config_json[u'daemon_mode'])
        except:
            self.init_config_err = u'daemon_mode is illegal'
            self.init_config_flag = False
            return

        # 检查daemon_interval
        try:
            int(self.config_json[u'daemon_interval'])
        except:
            self.init_config_err = u'daemon_interval is illegal'
            self.init_config_flag = False
            return

        # 检查daemon_interval
        try:
            int(self.config_json[u'enable_https'])
        except:
            self.init_config_err = u'enable_https is illegal'
            self.init_config_flag = False
            return

        # 设置db_path
        self.config_json[u'db_path'] = u'./db/db_rec.db'

        # 通过检查,配置文件有效
        self.init_config_flag = True
        self.init_config_err = u''

    # 返回配置是否有效
    def is_valid_config(self):
        return self.init_config_flag

    # 获取配置错误信息
    def get_err_msg(self):
        return self.init_config_err

    # 返回配置中的appid
    def get_appid(self):
        return self.config_json[u'appid']

    # 返回配置中的secret_id
    def get_secret_id(self):
        return self.config_json[u'secret_id']

    # 返回配置中的secret_key
    def get_secret_key(self):
        return self.config_json[u'secret_key']

    # 返回配置中的bucket
    def get_bucket(self):
        return self.config_json[u'bucket']

    # 返回配置中的要同步的本地目录或文件路径, 以/结尾
    def get_local_path(self):
        local_path = self.config_json[u'local_path']
        # 如果是windows的分隔符\, 则换为/
        if local_path[len(local_path) - 1] != u'/':
            local_path += u'/'
        return local_path

    # 返回配置中的同步到的cos目录, 以/结尾
    def get_cos_path(self):
        cos_path = self.config_json[u'cos_path']
        if cos_path[len(cos_path) - 1] != u'/':
            cos_path += u'/'
        return cos_path

    # 返回配置中的上传超时时间
    def get_timeout(self):
        return int(self.config_json[u'timeout'])

    # 获取同步的线程池数量
    def get_thread_num(self):
        return int(self.config_json[u'thread_num'])

    # 删除是否同步, 即用户删除了本地文件后, cos上是否删除
    def get_delete_sync(self):
        return int(self.config_json[u'delete_sync'])

    # 是否启动damon模式
    def get_daemon_mode(self):
        return int(self.config_json[u'daemon_mode'])

    # 获取damon执行的间隔时间, 单位秒
    def get_daemon_interval(self):
        return int(self.config_json[u'daemon_interval'])

    # 获取是否开启https
    def get_enable_https(self):
        return int(self.config_json[u'enable_https'])

    # 返回存取本地同步目录的数据库文件路径
    def get_db_path(self):
        return self.config_json[u'db_path']

################################################################################
# 封装有关文件和目录属性, 如文件大小, sha值等                                  #
################################################################################
class FileStat(object):
    def __init__(self, file_path=None):
        if file_path is None:
            self.valid = False
            return

        self.valid = True

        file_stat = os.stat(file_path)
        # 文件路径
        self.file_path = file_path
        # 文件模式
        self.file_mode = file_stat[stat.ST_MODE]
        # 文件大小
        self.file_size = file_stat[stat.ST_SIZE]
        # 文件最近修改时间
        self.mtime = file_stat[stat.ST_MTIME]
        # 文件的MD5值,默认是为空, 不进行计算, 只在需要的时候才进行计算
        self.md5 = u''

    # 判断是否是合法的要同步的文件, 包括目录或者普通文件
    # 其他类型的文件不进行同步
    def is_valid(self):
        return self.valid

    # 生成目录和文件的MD5值
    def build_md5(self):
        # 目录的MD5值, 直接返回空串
        if (stat.S_ISDIR(self.file_mode)):
            self.md5 = u''
            return

        md5_hash = hashlib.md5()
        file_obj = open(self.file_path, 'rb')
        try:
            while True:
                byte_in = file_obj.read(4096)
                if not byte_in:
                    break
                md5_hash.update(byte_in)
        except:
            return ""
        finally:
            file_obj.close()
        return md5_hash.hexdigest()

    # 根据属性生成FileStat, 用于从数据库读出属性生成FileStat
    @classmethod
    def build_stat(cls, file_path=u'', file_size=u'', file_mode=u'', mtime=u'',
            md5=u''):
        file_stat = FileStat()
        file_stat.file_path = file_path
        file_stat.file_size = file_size
        file_stat.file_mode = file_mode
        file_stat.mtime = mtime
        file_stat.md5 = md5
        file_stat.valid = True
        return file_stat

    # 针对路径一样的文件和目录进行比较, 判断是否发生了修改
    # 如果一个文件或者目录未发生改变, 则返回0
    # 否则返回1
    def compare(self, file_stat):
        # 如果两者是目录,则直接返回0(一样)
        if (stat.S_ISDIR(self.file_mode)):
            return 0
        # 对于文件, 我们需要首先比较mtime是否发生了改变
        if (self.mtime == file_stat.mtime):
            return 0
        # 如果文件大小发生了改变,则文件一定发生了变化
        if (self.file_size != file_stat.file_size):
            return 1
        # 如果文件大小不变, mtime发生了改变,则我们需要计算MD5
        self.md5 = self.build_md5()
        if (self.md5 != file_stat.md5):
            return 1

        return 0

################################################################################
# 记录本地要同步的文件和目录列表                                               #
################################################################################
class LocalFileDirInfo(object):
    def __init__(self, local_path = None):
        self.file_stat_list = []
        self.dir_stat_list  = []
        if local_path is None:
            return

        if os.path.isfile(local_path):
            self.file_stat_list.append(FileStat(local_path))
            return

        for parent, dirnames, filenames in os.walk(local_path):
            for filename in filenames:
                full_path = os.path.join(parent, filename)
                full_path = full_path.replace(u'\\', u'/')
                file_stat = FileStat(full_path)
                if not file_stat.valid:
                    print "wrong file_stat, full_path:", full_path
                else:
                    self.file_stat_list.append(file_stat)

            for dirname in dirnames:
                full_path = os.path.join(parent, dirname)
                full_path = full_path.replace(u'\\', u'/')
                file_stat = FileStat(full_path)
                if not file_stat.valid:
                    print "wrong file_stat, full_path:", full_path
                else:
                    self.dir_stat_list.append(file_stat)


################################################################################
# db init                                                                      #
################################################################################
class DbRecord(object):
    # 创建数据表, 表名是"cos_sync_table"与用户上传目的地的md5的base64的字符串拼接
    def __init__(self, db_path, appid, bucket, cos_path):
        # connect sqlite db
        self.cx = sqlite3.connect(db_path, check_same_thread = False)
        # 如果表不存在,则建表
        md5_hash = hashlib.md5()
        md5_hash.update(cos_path)
        md5_digest = md5_hash.hexdigest()

        self.table_name = u'cos_sync_table_' + appid + u'_' + bucket + u'_' + md5_digest

        sql_create_table = u'create table if not exists ' + self.table_name + u'''
        (`id` INTEGER PRIMARY key autoincrement,
        `file_path` varchar(1024) default '',
        `file_size` bigint not null default 0,
        `file_mode` bigint not null default 0,
        `mtime` bigint not null,
        `md5` char(32) default '',
        unique (`file_path`))'''

        self.cx.execute(sql_create_table)
        self.cx.commit()
        self.file_stat_dict = {}
        self.dir_stat_dict = {}
        # 获取已经存在的文件列表
        sql_select_all = u'select * from ' + self.table_name
        cursor = self.cx.execute(sql_select_all)
        for element in cursor:
            file_path = element[1]
            file_size = element[2]
            file_mode = element[3]
            mtime     = element[4]
            md5       = element[5]
            if (stat.S_ISREG(file_mode)):
                self.file_stat_dict[file_path] = FileStat.build_stat(file_path,
                        file_size, file_mode, mtime, md5)
            else:
                self.dir_stat_dict[file_path] = FileStat.build_stat(file_path,
                        file_size, file_mode, mtime, md5)

        # 初始化一个数据库操作锁, 避免多线程写操作
        self.db_lock = threading.Lock()

    # 关闭数据库连接
    def __del__(self):
        if self.db_lock.acquire():
            self.cx.close()
            self.db_lock.release()

    # 更新记录
    # 如果存在file_path同样的记录则更新,否则插入一条新的记录
    def update_record(self, file_stat):
        # 插入之前判断下md5是否计算过，没有则进行计算
        if (len(file_stat.md5) == 0):
            file_stat.md5 = file_stat.build_md5()

        record_tuple = (self.table_name, file_stat.file_path, file_stat.file_size,
                file_stat.file_mode, file_stat.mtime, file_stat.md5)
        sql_update_record = u'''
        replace into %s
        (file_path, file_size, file_mode, mtime, md5)
        values
        ('%s', %d, %d, %d, '%s')''' % record_tuple
        if self.db_lock.acquire():
            self.cx.execute(sql_update_record)
            self.cx.commit()
            self.db_lock.release()

    # 删除记录
    def del_record(self, file_stat):
        del_tuple = (self.table_name, file_stat.file_path)
        sql_delete_record = u"delete from %s where file_path = '%s'" % del_tuple
        if self.db_lock.acquire():
            self.cx.execute(sql_delete_record)
            self.cx.commit()
            self.db_lock.release()

################################################################################
# cos任务统计
################################################################################
class CosTaskStatics(object):
    def __init__(self):
        self.lock = threading.Lock()
        self.statics_dict = {}

    # 更新统计项, 不存在初始化为1, 否则递增
    def update_statics(self, statics_name):
        if self.lock.acquire():
            if not self.statics_dict.has_key(statics_name):
                self.statics_dict[statics_name] = 1
            else:
                self.statics_dict[statics_name] += 1
            self.lock.release()

    # 清空缓存
    def clear_statics(self):
        if self.lock.acquire():
            self.statics_dict = {}
            self.lock.release()

    # 打印统计项
    def print_statics(self):
        if (len(self.statics_dict) == 0):
            print 'no any operation!'
        else:
            print "operation statics below:"
            for key, count in self.statics_dict.items():
                print '%30s : %d' % (key, count)

# 定义一个全局的统计对象
cos_task_statics = CosTaskStatics()

################################################################################
# cos任务函数                                                                  #
################################################################################
# 将cos server返回的http结果转换成可阅读的字符串
def convert_op_ret_str(op_ret):
    op_ret_str = '{'
    first_element = True
    for key, value in op_ret.items():
        if not first_element:
            op_ret_str += ', '
        if (type(value).__name__ == 'unicode'):
            element_str = "'%s':'%s'" % (key, value.encode("utf-8"))
        else:
            element_str = "'%s':'%s'" % (key, value)
        op_ret_str += element_str
        first_element = False

    op_ret_str += '}'
    return op_ret_str

# 判断对cos的操作是否成功, cos的默认回复是dict类型
def cos_op_success(cos_ret_dict):
    if cos_ret_dict.has_key(u'code'):
        # 0表示成功, 4018表示相同的文件已经上传过
        if cos_ret_dict[u'code'] == 0 or cos_ret_dict[u'code'] == -4018:
            return True
    return False

# 打印op成功的info消息
def print_op_ok(op, op_ret, local_path):
    global cos_sync_logger
    op_ret_str = convert_op_ret_str(op_ret)
    cos_sync_logger.info("[ok]   [%13s] [%20s] [%s]" % (op, op_ret_str, local_path))
    print '[ok]   [%13s] [%s]' % (op, local_path)

# 打印op失败的info消息
def print_op_fail(op, op_ret, local_path):
    global cos_sync_logger
    op_ret_str = convert_op_ret_str(op_ret)
    cos_sync_logger.info("[fail] [%13s] [%20s] [%s]" % (op, op_ret_str, local_path))
    print '[fail]   [%13s] [%s]' % (op, local_path)

# 判断目录是否存在, 存在返回True, 否则进行创建,并返回创建结果
# 这个用来创建COS上传的目的目录, 即配置文件中的cos_path
def create_folder_if_not_exist(cos_client, bucket, cos_dir):
    global cos_task_statics
    if cos_dir[len(cos_dir) - 1] != u'/':
        cos_dir += u'/'
    request = StatFolderRequest(bucket, cos_dir)
    op_ret = cos_client.stat_folder(request)
    if cos_op_success(op_ret):
        return True
    request = CreateFolderRequest(bucket, cos_dir)
    op_ret = cos_client.create_folder(request)
    if cos_op_success(op_ret):
        cos_task_statics.update_statics("create_folder_success")
        print_op_ok("createTargetFolder", op_ret, cos_dir)
        return True
    else:
        cos_task_statics.update_statics("create_folder_fail")
        print_op_fail("createTargetFolder", op_ret, cos_dir)
        return False

# 创建目录, 成功返回True, 失败返回False, 串行执行
def create_folder(cos_client, bucket, cos_path, db, file_stat):
    global cos_task_statics
    local_path = file_stat.file_path
    if cos_path[len(cos_path) - 1] != u'/':
        cos_path += u'/'
    request = CreateFolderRequest(bucket, cos_path)
    op_ret = cos_client.create_folder(request)
    if cos_op_success(op_ret):
        db.update_record(file_stat)
        cos_task_statics.update_statics("create_folder_success")
        print_op_ok("createFolder", op_ret, local_path)
        return True
    else:
        cos_task_statics.update_statics("create_folder_fail")
        print_op_fail("createFolder", op_ret, local_path)
        return False

# 删除目录, 成功返回True, 失败返回False, 串行执行
def delete_folder(cos_client, bucket, cos_path, db, file_stat):
    global cos_task_statics
    local_path = file_stat.file_path
    if cos_path[len(cos_path) - 1] != u'/':
        cos_path += u'/'
    request = DelFolderRequest(bucket, cos_path)
    op_ret = cos_client.del_folder(request)
    if cos_op_success(op_ret):
        db.del_record(file_stat)
        cos_task_statics.update_statics("delete_folder_success")
        print_op_ok("deleteFolder", op_ret, local_path)
        return True
    else:
        cos_task_statics.update_statics("delete_folder_fail")
        print_op_fail("deleteFolder", op_ret, local_path)
        return False

# 上传文件, 成功返回True, 失败返回False, 并行执行
def upload_file(cos_client, bucket, cos_path, db, file_stat):
    global cos_task_statics
    local_path = file_stat.file_path
    request = UploadFileRequest(bucket, cos_path, local_path)
    request.set_insert_only(0)      # 如果cos上有路径则覆盖文件
    op_ret = cos_client.upload_file(request)
    if cos_op_success(op_ret):
        db.update_record(file_stat)
        cos_task_statics.update_statics("upload_file_success")
        print_op_ok("uploadFile", op_ret, local_path)
        return True
    else:
        # cos_task_statics.update_statics("upload_file_fail")
        # print_op_fail("uploadFile", op_ret, local_path)
        # return False
        cos_task_statics.update_statics("upload_file_retries")
        print_op_fail("uploadFile (to retry)", op_ret, local_path)
        delete_file(cos_client, bucket, cos_path, db, file_stat)
        time.sleep(1)
        return upload_file(cos_client, bucket, cos_path, db, file_stat)

# 删除文件, 成功返回True, 失败返回False, 并行执行
def delete_file(cos_client, bucket, cos_path, db, file_stat):
    global cos_task_statics
    local_path = file_stat.file_path
    request = DelFileRequest(bucket, cos_path)
    op_ret = cos_client.del_file(request)
    if cos_op_success(op_ret):
        db.del_record(file_stat)
        cos_task_statics.update_statics("delete_file_success")
        print_op_ok("deleteFile", op_ret, local_path)
        return True
    else:
        cos_task_statics.update_statics("delete_file_fail")
        print_op_fail("deleteFile", op_ret, local_path)
        return False

################################################################################
# 主类, 执行本地状态和数据库记录的cos上的状态的比较,并执行相应的动作           #
################################################################################
class CosSync(object):
    def __init__(self, config):
        self.config = config
        # 数据库文件记录的同步到cos上的文件
        self.db = DbRecord(config.get_db_path(), config.get_appid(),
                config.get_bucket(), config.get_cos_path())

    # 把本地路径转成cos路径
    def _local_path_to_cos(self, local_path):
        local_path = str(local_path)
        cos_dir    = self.config.get_cos_path()
        local_dir  = self.config.get_local_path()
        cos_path   = cos_dir + local_path[len(local_dir):]
        return cos_path

    # 检查同步的cos目录是否存在, 如果不存在, 则创建目录
    # 存在或者创建成功返回True, 否则返回False
    def _create_cos_target_dir(self, cos_client, bucket):
        cos_dir = self.config.get_cos_path()
        return create_folder_if_not_exist(cos_client, bucket, cos_dir)

    # 同步删除的文件, 使用线程池同步删除
    def _del_cos_sync_file(self, cos_client, bucket, del_file_dict):
        global delete_file
        # 如果没有任何需要删除的文件,则返回
        if (len(del_file_dict) == 0):
            return
        # 如果删除不同步, 则返回
        if self.config.get_delete_sync() == 0:
            return

        # 初始化一个删除线程池
        thread_worker_num = self.config.get_thread_num()
        thread_pool = threadpool.ThreadPool(thread_worker_num)
        for del_file_path in del_file_dict.keys():
            cos_path = self._local_path_to_cos(del_file_path)
            del_args = [cos_client, bucket, cos_path, self.db,
                    del_file_dict[del_file_path]]
            args_tuple = (del_args, None)
            args_list = [args_tuple]
            requests = threadpool.makeRequests(delete_file, args_list)
            for req in requests:
                thread_pool.putRequest(req)
        # 等待线程执行结束
        thread_pool.wait()
        thread_pool.dismissWorkers(thread_worker_num, True)

    # 同步删除的目录, 串行删除, 保证删除父子目录的顺序
    def _del_cos_sync_dir(self, cos_client, bucket, del_dir_dict):
        global delete_folder
        # 如果没有任何需要删除的目录, 则返回
        if (len(del_dir_dict) == 0):
            return
        # 如果删除不同步, 则返回
        if self.config.get_delete_sync() == 0:
            return

        # 对del_dir_dict按照降序排序,即先删除子目录,再删父目录
        for del_dir_path in sorted(del_dir_dict.keys(), reverse=True):
            # 本地路径转换成cos的路径
            cos_path = self._local_path_to_cos(del_dir_path)
            delete_folder(cos_client, bucket, cos_path, self.db,
                    del_dir_dict[del_dir_path])

    # 同步创建的文件, 使用多线程创建
    def _create_cos_sync_file(self, cos_client, bucket, create_file_dict):
        global upload_file
        # 如果没有任何需要上传的文件, 则返回
        if (len(create_file_dict) == 0):
            return
        # 初始化一个删除线程池
        thread_worker_num = self.config.get_thread_num()
        thread_pool = threadpool.ThreadPool(thread_worker_num)
        for create_file_path in create_file_dict.keys():
            #本地路径转换成cos的路径
            cos_path = self._local_path_to_cos(create_file_path)
            upload_args = [cos_client, bucket, cos_path, self.db,
                    create_file_dict[create_file_path]]
            args_tuple = (upload_args, None)
            args_list = [args_tuple]
            requests = threadpool.makeRequests(upload_file, args_list)
            for req in requests:
                thread_pool.putRequest(req)

        # 等待线程执行结束
        thread_pool.wait()
        thread_pool.dismissWorkers(thread_worker_num, True)

    # 同步创建的目录, 使用串行创建, 避免父子目录乱序创建
    def _create_cos_sync_dir(self, cos_client, bucket, create_dir_dict):
        global create_folder
        # 如果没有任何需要创建的目录,则返回
        if (len(create_dir_dict) == 0):
            return
        # 对crate_dir_dict按照升序排序, 即先建父目录, 再建子目录
        for create_dir_path in sorted(create_dir_dict.keys(), reverse=False):
            cos_path = self._local_path_to_cos(create_dir_path)
            create_folder(cos_client, bucket, cos_path, self.db,
                    create_dir_dict[create_dir_path])

    def sync(self):
        # 本地文件目录列表
        local_file_dir_info = LocalFileDirInfo(self.config.get_local_path())

        # 记录的要删除的文件和目录列表
        del_file_dict = self.db.file_stat_dict
        del_dir_dict  = self.db.dir_stat_dict

        # 记录的要创建的文件和目录列表
        create_file_dict = {}
        create_dir_dict  = {}

        # 文件同步状态校验
        for local_file_stat in local_file_dir_info.file_stat_list:
            local_file_path = local_file_stat.file_path
            # 如果数据库中记录cos中也存在, 则判断下文件是否发生了改变
            if (self.db.file_stat_dict.has_key(local_file_path)):
                # 如果文件内容未发生变化, 则不用同步, 否则进行同步
                if (FileStat.compare(local_file_stat,
                    self.db.file_stat_dict[local_file_path]) == 0):
                    del self.db.file_stat_dict[local_file_path]
                else:
                    create_file_dict[local_file_path] = local_file_stat
            # 如果数据库中不存在相关记录, 则插入到要创建的文件列表
            else:
                create_file_dict[local_file_path] = local_file_stat

        # 目录同步状态校验
        for local_dir_stat in local_file_dir_info.dir_stat_list:
            local_dir_path = local_dir_stat.file_path
            # 如果数据库中记录cos中也存在, 则判断下目录是否发生了改变
            if (self.db.dir_stat_dict.has_key(local_dir_path)):
                del self.db.dir_stat_dict[local_dir_path]
            # 如果数据库中不存在相关记录, 则插入到要创建的文件列表
            else:
                create_dir_dict[local_dir_path] = local_dir_stat

        # 生成cos客户端
        appid      = int(self.config.get_appid())
        secret_id  = self.config.get_secret_id()
        secret_key = self.config.get_secret_key()
        bucket     = self.config.get_bucket()
        timeout    = self.config.get_timeout()
        cos_client = CosClient(appid, secret_id, secret_key)
        cos_config = CosConfig()
        cos_config.set_timeout(timeout)
        if self.config.get_enable_https() == 1:
            cos_config.enable_https()
        cos_client.set_config(cos_config)

        # 创建要同步的cos 目录
        self._create_cos_target_dir(cos_client, bucket)

        # 进行同步, 同步顺序如下
        # 1 删除文件(之前本地有，现在没有的文件, 或者发生修改的文件)
        self._del_cos_sync_file(cos_client, bucket, del_file_dict)

        # 2 删除目录(之前本地有，现在没有的目录)
        self._del_cos_sync_dir(cos_client, bucket, del_dir_dict)

        # 3 创建目录(新建的目录)
        self._create_cos_sync_dir(cos_client, bucket, create_dir_dict)

        # 4 创建文件(新建的文件)
        self._create_cos_sync_file(cos_client, bucket, create_file_dict)

################################################################################
# main                                                                         #
################################################################################
if __name__ == '__main__':
    # 设置默认编码为utf-8
    reload(sys)
    sys.setdefaultencoding('utf-8')
    # 读取配置文件, 并判断
    config = CosSyncConfig('./conf/config.json')
    if not config.is_valid_config():
        print 'wrong config: ' + config.get_err_msg()
        sys.exit(1)
    while True:
        start = time.time()
        cos_sync = CosSync(config)
        cos_sync.sync()
        stop = time.time()
        cos_task_statics.print_statics()
        cos_task_statics.clear_statics()
        print '\nsync over! used time:%.2f s' % (stop - start)
        if config.get_daemon_mode() == 1:
            daemon_interval = config.get_daemon_interval()
            time.sleep(daemon_interval)
        else:
            break;
