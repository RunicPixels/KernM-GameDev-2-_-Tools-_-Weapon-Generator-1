namespace RubicalMe
{
	public class EditorRendererEvent
	{
		public enum EventType
		{
			clicked
		}

		public EditorRendererEvent (uint buttonID, EventType eventType)
		{
			this.buttonID = buttonID;
			this.type = eventType;
		}

		public readonly uint buttonID;
		public readonly EventType type;
	}
}
