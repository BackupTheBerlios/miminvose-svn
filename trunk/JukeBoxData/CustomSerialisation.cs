using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JukeBoxData
{
	public sealed class CustomSerialisationHelper
	{
		Stream _stream;

		public CustomSerialisationHelper(Stream stream)
		{
			_stream = stream;
		}

		public void Write(string[] slist)
		{
			WriteArrayLength(slist.Length);
			foreach (string s in slist)
			{
				Write(s);
			}
		}

		public void Write(List<string> slist)
		{
			WriteArrayLength(slist.Count);
			foreach (string s in slist)
			{
				Write(s);
			}
		}

		public void Write(String s)
		{
			if (s == null) Write((byte[])null);
			else Write(Encoding.UTF8.GetBytes(s));
		}

		public void Write(byte[] bytes)
		{
			if (bytes == null) WriteArrayLength(0);
			else
			{
				WriteArrayLength(bytes.Length);
				_stream.Write(bytes, 0, bytes.Length);
			}
		}

		public void WriteArrayLength(int length)
		{
			if (length < 0) throw new ArgumentOutOfRangeException("length", "encountered negative array size");
			if (length > ushort.MaxValue)
			{
				Write((byte)4);
				Write((uint)length);
			}
			else if (length > byte.MaxValue)
			{
				Write((byte)2);
				Write((ushort)length);
			}
			else
			{
				Write((byte)1);
				Write((byte)length);
			}
		}

		public void Write(DateTime value)
		{
			Write(DateHelper.GetSeconds(value));
		}

		public void Write(Boolean value)
		{
			if (value) Write((Byte)1);
			else Write((Byte)0);
		}

		public void Write(System.Byte value)
		{
			byte[] bytes = BytesHelper.ToBytes(value);
			_stream.Write(bytes, 0, bytes.Length);
		}

		public void Write(System.UInt16 value)
		{
			byte[] bytes = BytesHelper.ToBytes(value);
			_stream.Write(bytes, 0, bytes.Length);
		}

		public void Write(System.UInt32 value)
		{
			byte[] bytes = BytesHelper.ToBytes(value);
			_stream.Write(bytes, 0, bytes.Length);
		}

		public void Write(System.UInt64 value)
		{
			byte[] bytes = BytesHelper.ToBytes(value);
			_stream.Write(bytes, 0, bytes.Length);
		}

		public void Write(System.SByte value)
		{
			_stream.WriteByte((byte)value);
		}

		public void Write(System.Int16 value)
		{
			byte[] bytes = BytesHelper.ToBytes(value);
			_stream.Write(bytes, 0, bytes.Length);
		}

		public void Write(System.Int32 value)
		{
			byte[] bytes = BytesHelper.ToBytes(value);
			_stream.Write(bytes, 0, bytes.Length);
		}

		public void Write(System.Int64 value)
		{
			byte[] bytes = BytesHelper.ToBytes(value);
			_stream.Write(bytes, 0, bytes.Length);
		}
	}

	public sealed class CustomDeserialisationHelper
	{
		Stream _stream;

		public CustomDeserialisationHelper(Stream stream)
		{
			_stream = stream;
		}

		private byte ReadByte()
		{
			int result = _stream.ReadByte();
			if (result < 0) throw new Exception("Stream exhausted");
			return (byte)result;
		}

		private byte[] Read(int size)
		{
			if (size == 0) return null;
			byte[] bytes = new byte[size];
			int count = _stream.Read(bytes, 0, size);
			if (count < size) throw new Exception("Stream exhausted");
			return bytes;
		}

		public DateTime ReadDateTime()
		{
			return DateHelper.GetDateTime(ReadInt64());
		}

		public byte ReadUInt08()
		{
			return BytesHelper.ToUInt08(ReadByte());
		}

		public UInt16 ReadUInt16()
		{
			return BytesHelper.ToUInt16(Read(BytesHelper.SIZEBYTES16), 0);
		}

		public UInt32 ReadUInt32()
		{
			return BytesHelper.ToUInt32(Read(BytesHelper.SIZEBYTES32), 0);
		}

		public UInt64 ReadUInt64()
		{
			return BytesHelper.ToUInt64(Read(BytesHelper.SIZEBYTES64), 0);
		}

		public sbyte ReadInt08()
		{
			return BytesHelper.ToInt08(ReadByte());
		}

		public Int16 ReadInt16()
		{
			return BytesHelper.ToInt16(Read(BytesHelper.SIZEBYTES16), 0);
		}

		public Int32 ReadInt32()
		{
			return BytesHelper.ToInt32(Read(BytesHelper.SIZEBYTES32), 0);
		}

		public Int64 ReadInt64()
		{
			return BytesHelper.ToInt64(Read(BytesHelper.SIZEBYTES64), 0);
		}

		public bool ReadBoolean()
		{
			return (ReadUInt08() == 1);
		}

		public string ReadString()
		{
			byte[] bytes = ReadBytes();
			if (bytes == null) return null;
			return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
		}

		public byte[] ReadBytes()
		{
			return Read(ReadArrayLength());
		}

		public int ReadArrayLength()
		{
			switch (ReadUInt08())
			{
				case 1:
					return (int)ReadUInt08();
				case 2:
					return (int)ReadUInt16();
				case 3:
					return (int)ReadUInt32();
				default:
					throw new Exception("Read invalid value for array length size");
			}
		}

		public string[] ReadStringArray()
		{
			int length = ReadArrayLength();
			string[] strings = new string[length];
			for (int i = 0; i < length; i++)
			{
				strings[i] = ReadString();
			}
			return strings;
		}

		public List<string> ReadStringList()
		{
			int length = ReadArrayLength();
			List<string> strings = new List<string>(length);
			for (int i = 0; i < length; i++)
			{
				strings.Add(ReadString());
			}
			return strings;
		}
	}

	public static class BytesHelper
	{
		public static readonly ushort SIZEBYTES08 = 1;
		public static readonly ushort SIZEBYTES16 = 2;
		public static readonly ushort SIZEBYTES32 = 4;
		public static readonly ushort SIZEBYTES64 = 8;

		public static void Write(Stream stream, Byte value)
		{
			byte[] bytes = ToBytes(value);
			stream.Write(bytes, 0, bytes.Length);
		}

		public static void Write(Stream stream, UInt16 value)
		{
			byte[] bytes = ToBytes(value);
			stream.Write(bytes, 0, bytes.Length);
		}

		public static void Write(Stream stream, UInt32 value)
		{
			byte[] bytes = ToBytes(value);
			stream.Write(bytes, 0, bytes.Length);
		}

		public static void Write(Stream stream, UInt64 value)
		{
			byte[] bytes = ToBytes(value);
			stream.Write(bytes, 0, bytes.Length);
		}

		public static void Write(Stream stream, SByte value)
		{
			byte[] bytes = ToBytes(value);
			stream.Write(bytes, 0, bytes.Length);
		}

		public static void Write(Stream stream, Int16 value)
		{
			byte[] bytes = ToBytes(value);
			stream.Write(bytes, 0, bytes.Length);
		}

		public static void Write(Stream stream, Int32 value)
		{
			byte[] bytes = ToBytes(value);
			stream.Write(bytes, 0, bytes.Length);
		}

		public static void Write(Stream stream, Int64 value)
		{
			byte[] bytes = ToBytes(value);
			stream.Write(bytes, 0, bytes.Length);
		}

		public static byte[] ToBytes(Byte value)
		{
			return ToBytes((UInt64)value, SIZEBYTES08);
		}

		public static byte[] ToBytes(UInt16 value)
		{
			return ToBytes((UInt64)value, SIZEBYTES16);
		}

		public static byte[] ToBytes(UInt32 value)
		{
			return ToBytes((UInt64)value, SIZEBYTES32);
		}

		public static byte[] ToBytes(UInt64 value)
		{
			return ToBytes(value, SIZEBYTES64);
		}

		private static byte[] ToBytes(UInt64 value, ushort numBytes)
		{
			byte[] result = new byte[numBytes];

			for (int i = numBytes - 1; i >= 0; i--)
			{
				byte b = (byte)(0xFFL & value);
				result[i] = (byte)(b + sbyte.MinValue);
				value >>= 8;
			}

			return result;
		}

		public static byte[] ToBytes(SByte value)
		{
			return BitConverter.GetBytes(value);
		}

		public static byte[] ToBytes(Int16 value)
		{
			Byte[] bytes = BitConverter.GetBytes(value);
			Reverse(bytes);
			return bytes;
		}

		public static byte[] ToBytes(Int32 value)
		{
			Byte[] bytes = BitConverter.GetBytes(value);
			Reverse(bytes);
			return bytes;
		}

		public static byte[] ToBytes(Int64 value)
		{
			Byte[] bytes = BitConverter.GetBytes(value);
			Reverse(bytes);
			return bytes;
		}

		public static Byte ToUInt08(byte b)
		{
			return (Byte)((sbyte)b - sbyte.MinValue);
		}

		public static Byte ToUInt08(byte[] bytes, uint startByte)
		{
			return ToUInt08(bytes[startByte]);
		}

		public static UInt16 ToUInt16(byte[] bytes, uint startByte)
		{
			return (UInt16)ToUInt64(bytes, SIZEBYTES16, startByte);
		}

		public static UInt32 ToUInt32(byte[] bytes, uint startByte)
		{
			return (UInt32)ToUInt64(bytes, SIZEBYTES32, startByte);
		}

		public static UInt64 ToUInt64(byte[] bytes, uint startByte)
		{
			return ToUInt64(bytes, SIZEBYTES64, startByte);
		}

		public static UInt64 ToUInt64(byte[] bytes, ushort numBytes, uint startByte)
		{
			UInt64 result = 0;

			for (uint i = startByte; i < (startByte + numBytes); i++)
			{
				UInt64 b = (UInt64)((sbyte)bytes[i] - sbyte.MinValue);
				result = (result << 8) + b;
			}

			return result;
		}

		public static SByte ToInt08(byte b)
		{
			return (sbyte)b;
		}

		public static SByte ToInt08(byte[] bytes, uint start)
		{
			return ToInt08(bytes[start]);
		}

		public static Int16 ToInt16(byte[] bytes, uint start)
		{
			//return BitConverter.ToInt16(bytes, (int)start);
			return BitConverter.ToInt16(BuildReverse(bytes, start, SIZEBYTES16), 0);
		}

		public static Int32 ToInt32(byte[] bytes, uint start)
		{
			//return BitConverter.ToInt32(bytes, (int)start);
			return BitConverter.ToInt32(BuildReverse(bytes, start, SIZEBYTES32), 0);
		}

		public static Int64 ToInt64(byte[] bytes, uint start)
		{
			//return BitConverter.ToInt64(bytes, (int)start);
			return BitConverter.ToInt64(BuildReverse(bytes, start, SIZEBYTES64), 0);
		}

		public static void Reverse(byte[] bytes)
		{
			if ((bytes == null) || (bytes.Length < 2)) return;
			for (int i = 0; i < bytes.Length / 2; i++)
			{
				int j = (bytes.Length - 1) - i;
				byte temp = bytes[i];
				bytes[i] = bytes[j];
				bytes[j] = temp;
			}
		}

		public static byte[] BuildReverse(byte[] bytes, uint start, uint count)
		{
			if (bytes == null) return null;
			byte[] copy = new byte[count];
			for (int i = 0; i < count; i++)
			{
				int j = (int)count - (i + 1);
				copy[j] = bytes[start + i];
			}
			return copy;
		}
	}

	public static class DateHelper
	{
		private static readonly DateTime EPOCH = new DateTime(2040, 07, 15, 11, 30, 0, 0, DateTimeKind.Utc);
		private static readonly long EPOCHSECONDS = EPOCH.Ticks / TimeSpan.TicksPerSecond;

		public static DateTime GetDateTime(long seconds)
		{
			DateTime utc = new DateTime((EPOCHSECONDS + seconds) * TimeSpan.TicksPerSecond);
			return utc.ToLocalTime();
		}

		public static long GetSeconds(DateTime date)
		{
			long seconds = date.ToUniversalTime().Ticks / TimeSpan.TicksPerSecond;
			return (seconds - EPOCHSECONDS);
		}
	}
}
