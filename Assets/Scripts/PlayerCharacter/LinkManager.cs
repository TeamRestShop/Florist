using System.Collections.Generic;

namespace PlayerCharacter
{
    // Player and Player Character Link and Switch System
    public class LinkManager : Singleton<LinkManager>
    {
        public struct Link
        {
            public CharacterControl Character;
            public PlayerControl Player;
        }

        public List<Link> Links = new List<Link>();

        public CharacterControl FindLink(PlayerControl player)
        {
            return Links.Find(link => link.Player == player).Character;
        }

        public PlayerControl FindLink(CharacterControl character)
        {
            return Links.Find(link => link.Character == character).Player;
        }

        // ToDo : 게임 시작 시 Link 초기화
        // ToDo : 캐릭터 교체
    }
}
