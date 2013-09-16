namespace DeltaEngine.Networking.Mocks
{
	/// <summary>
	/// Provides servers and clients for network unit testing with mocks. 
	/// When resolving a client it will always connect to the last MockServer created, regardless of
	/// the hostname provided in Client.Connect
	/// </summary>
	internal class MockNetworkResolver : NetworkResolver
	{
		public Server ResolveServer()
		{
			lastMockServer = new MockServer();
			return lastMockServer;
		}

		private MockServer lastMockServer;

		public Client ResolveClient()
		{
			return new MockClient(lastMockServer);
		}
	}
}