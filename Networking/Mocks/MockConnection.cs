namespace DeltaEngine.Networking.Mocks
{
	public class MockConnection : MockClient
	{
		/// <summary>
		/// Connection to a mock client during a unit test.
		/// </summary>
		public MockConnection(MockClient client)
			: base(null)
		{
			Client = client;
			//client.DataSent += message => Receive(message);
			DataSent += message => client.Receive(message);
		}

		public MockClient Client { get; private set; }
	}
}