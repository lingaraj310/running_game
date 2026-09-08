namespace Dreamers.Utilities
{
    /// <summary>
    /// Centralized gameplay constants for Dreamers — Run Beyond Limits.
    /// Eliminates magic numbers and standardizes lane coordinates, physics settings, tags, and scene names.
    /// </summary>
    public static class GameConstants
    {
        #region Lane System Constants
        public const float LANE_WIDTH = 2.0f;
        public const float LEFT_LANE_X = -LANE_WIDTH;
        public const float CENTER_LANE_X = 0.0f;
        public const float RIGHT_LANE_X = LANE_WIDTH;

        public const int LANE_INDEX_LEFT = 0;
        public const int LANE_INDEX_CENTER = 1;
        public const int LANE_INDEX_RIGHT = 2;
        public const int DEFAULT_LANE_INDEX = LANE_INDEX_CENTER;
        public const int TOTAL_LANES = 3;
        #endregion

        #region Movement & Physics Defaults
        public const float DEFAULT_RUN_SPEED = 10.0f;
        public const float MIN_RUN_SPEED = 6.0f;
        public const float MAX_RUN_SPEED_CAP = 25.0f;

        public const float LANE_SWITCH_DURATION = 0.18f;
        public const float JUMP_FORCE = 8.5f;
        public const float GRAVITY_SCALE = 24.0f;
        public const float SLIDE_DURATION = 0.75f;
        public const float SLIDE_COLLIDER_HEIGHT_RATIO = 0.5f;

        public const float GROUND_CHECK_DISTANCE = 0.15f;
        #endregion

        #region Input & Gesture Defaults
        public const float DEFAULT_MIN_SWIPE_DISTANCE_INCHES = 0.35f;
        public const float MAX_SWIPE_TIME_SECONDS = 0.5f;
        #endregion

        #region Tags and Layers
        public const string TAG_PLAYER = "Player";
        public const string TAG_OBSTACLE = "Obstacle";
        public const string TAG_COLLECTIBLE = "Collectible";
        public const string TAG_POWERUP = "PowerUp";
        public const string TAG_TRACK_SEGMENT = "TrackSegment";

        public const string LAYER_PLAYER = "Player";
        public const string LAYER_OBSTACLE = "Obstacle";
        public const string LAYER_GROUND = "Ground";
        #endregion

        #region Scene Names
        public const string SCENE_BOOT = "SCN_Boot";
        public const string SCENE_MAIN_MENU = "SCN_MainMenu";
        public const string SCENE_GAMEPLAY = "SCN_Gameplay";
        #endregion
    }
}
