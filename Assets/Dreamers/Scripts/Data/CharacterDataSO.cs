using UnityEngine;
using Dreamers.Utilities;

namespace Dreamers.Data
{
    /// <summary>
    /// Static configuration for a playable character in Dreamers.
    /// Defines identity, theme, visual assets, movement tuning, and ability parameters.
    /// </summary>
    [CreateAssetMenu(fileName = "SO_Char_NewCharacter", menuName = "Dreamers/Data/Character Data")]
    public class CharacterDataSO : ScriptableObject
    {
        [Header("Character Identity")]
        [SerializeField] private string _characterId;
        [SerializeField] private string _displayName;
        [SerializeField] private string _careerTheme;
        [TextArea(2, 4)]
        [SerializeField] private string _description;
        [SerializeField] private Sprite _portraitIcon;

        [Header("Visual & Animation References")]
        [SerializeField] private GameObject _characterPrefab;
        [SerializeField] private RuntimeAnimatorController _animatorController;

        [Header("Movement & Physics Tuning")]
        [SerializeField] private float _baseRunSpeed = GameConstants.DEFAULT_RUN_SPEED;
        [SerializeField] private float _laneSwitchDuration = GameConstants.LANE_SWITCH_DURATION;
        [SerializeField] private float _jumpForce = GameConstants.JUMP_FORCE;
        [SerializeField] private float _gravityScale = GameConstants.GRAVITY_SCALE;
        [SerializeField] private float _slideDuration = GameConstants.SLIDE_DURATION;

        [Header("Special Ability Metadata")]
        [SerializeField] private string _abilityId;
        [SerializeField] private string _abilityName;
        [TextArea(1, 3)]
        [SerializeField] private string _abilityDescription;
        [SerializeField] private float _abilityCooldown = 15.0f;
        [SerializeField] private float _abilityDuration = 5.0f;

        [Header("Unlock Requirements")]
        [SerializeField] private bool _isUnlockedByDefault;
        [SerializeField] private int _coinUnlockCost;

        #region Public Getters
        public string CharacterId => _characterId;
        public string DisplayName => _displayName;
        public string CareerTheme => _careerTheme;
        public string Description => _description;
        public Sprite PortraitIcon => _portraitIcon;

        public GameObject CharacterPrefab => _characterPrefab;
        public RuntimeAnimatorController AnimatorController => _animatorController;

        public float BaseRunSpeed => _baseRunSpeed;
        public float LaneSwitchDuration => _laneSwitchDuration;
        public float JumpForce => _jumpForce;
        public float GravityScale => _gravityScale;
        public float SlideDuration => _slideDuration;

        public string AbilityId => _abilityId;
        public string AbilityName => _abilityName;
        public string AbilityDescription => _abilityDescription;
        public float AbilityCooldown => _abilityCooldown;
        public float AbilityDuration => _abilityDuration;

        public bool IsUnlockedByDefault => _isUnlockedByDefault;
        public int CoinUnlockCost => _coinUnlockCost;
        #endregion
    }
}
