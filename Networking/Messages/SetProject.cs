namespace DeltaEngine.Networking.Messages
{
	/// <summary>
	/// Server sends this to the client initially after Login to give the client the project name
	/// </summary>
	public class SetProject
	{
		private SetProject() {}

		public SetProject(string projectName, ProjectPermissions permissions)
		{
			ProjectName = projectName;
			Permissions = permissions;
		}

		public string ProjectName { get; private set; }
		public ProjectPermissions Permissions { get; private set; }
	}
}