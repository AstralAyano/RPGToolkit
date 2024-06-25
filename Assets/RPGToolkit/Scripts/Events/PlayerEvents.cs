using System;

namespace RPGToolkit
{
    public class PlayerEvents
    {
        public event Action<int> onPlayerLevelChange;
        public void PlayerLevelChange(int level) 
        {
            if (onPlayerLevelChange != null) 
            {
                onPlayerLevelChange(level);
            }
        }
    }
}
