using SolarSystemKritskiy_Data;
using UnityEngine;

namespace SolarSystemKritskiy_Main
{
    public class SettingsContainer : Singleton<SettingsContainer>
    {
        public SpaceShipSettings SpaceShipSettings => _spaceShipSettings;

        [SerializeField] private SpaceShipSettings _spaceShipSettings;
    }
}