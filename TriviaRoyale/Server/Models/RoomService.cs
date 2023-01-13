using TriviaRoyale.Shared;

namespace TriviaRoyale.Server.Models
{
    public class RoomService
    {
        public readonly List<GameRoom> rooms = new();
        public event Action OnChange;

        public GameRoom NewRoom()
        {
            GameRoom room = new();
            rooms.Add(room);
            NotifyStateChanged();
            return room;
        }

        public void RemoveRoom(GameRoom room)
        {
            rooms.Remove(room);
            NotifyStateChanged();
        }


        private void NotifyStateChanged() => OnChange?.Invoke();

    }
}
