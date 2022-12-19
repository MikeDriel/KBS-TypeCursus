using System.Diagnostics;
using Renci.SshNet;
using System.Net.Sockets;
using System.Reflection;


namespace Model
{
	public class SshTunnel : IDisposable
	{
		private SshClient client;
		private ForwardedPortLocal port;

		public SshTunnel()
		{
			try
			{
				client = new SshClient("145.44.233.125", "student", "KaasKnabbel123!");

				port = new ForwardedPortLocal(1433, "145.44.233.125", 1433);
				
				client.Connect();
				client.AddForwardedPort(port);
				port.Start();
			}
			catch
			{
				Dispose();
				throw;
			}
		}

		public void Dispose()
		{
			client.Dispose();
			port.Dispose();
		}
	}
}
