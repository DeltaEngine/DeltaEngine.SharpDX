using System;

namespace DeltaEngine.Networking.Tcp
{
	internal class MessageData
	{
		public MessageData(int dataLength)
		{
			Data = new byte[dataLength];
			readDataLength = 0;
		}

		public byte[] Data { get; private set; }
		private int readDataLength;
		
		public int ReadData(byte[] availableBytes, int offset, int availableBytesCurrentLength)
		{
			int allowedBytesToRead = Data.Length - readDataLength;
			if (availableBytesCurrentLength < allowedBytesToRead)
				allowedBytesToRead = availableBytesCurrentLength;
			Array.Copy(availableBytes, offset, Data, readDataLength, allowedBytesToRead);
			readDataLength += allowedBytesToRead;
			return allowedBytesToRead;
		}

		public bool IsDataComplete
		{
			get { return readDataLength == Data.Length; }
		}
	}
}