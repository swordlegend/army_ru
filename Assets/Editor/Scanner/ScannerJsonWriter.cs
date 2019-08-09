//
// ScannerJsonWriter.cs
//
// Author:
//       duwenjie
//

//
// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System;
using LBoot;
using System.Linq;
using System.Text.RegularExpressions;
using LitJson;
using System.Text;

public class ScannerJsonWriter
{
	protected StringBuilder sb = null;
	protected JsonWriter writer = null;
	
	public ScannerJsonWriter()
	{
		this.sb = new StringBuilder();
		this.writer = new JsonWriter(sb);
		this.writer.PrettyPrint = true;
	}
	
	public void WriteObjectStart()
	{
		this.writer.WriteObjectStart();
	}
	
	public void WriteObjectEnd()
	{
		this.writer.WriteObjectEnd();
	}
	
	public void WritePropertyName(string name)
	{
		this.writer.WritePropertyName(name);
	}
	
	public void WriteArrayStart()
	{
		this.writer.WriteArrayStart();
	}
	
	public void WriteArrayEnd()
	{
		this.writer.WriteArrayEnd();
	}

	public void WriteVector3(Vector3 vec)
	{
		this.writer.WriteArrayStart();
		this.writer.Write(vec.x);
		this.writer.Write(vec.y);
		this.writer.Write(vec.z);
		this.writer.WriteArrayEnd();
	}
	
	public void Write(string s)
	{
		this.writer.Write(s);
	}

	public string OutputString()
	{
		return this.sb.ToString();
	}

    public bool OutputToFile(string filename, string content)
    {
        string s = content;
        File.WriteAllText(filename, s);
        return true;
    }
	
	public bool OutputToFile(string filename)
	{
		string s = this.sb.ToString();
		File.WriteAllText(filename, s);
		return true;
	}
	
}

