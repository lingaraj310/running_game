using UnityEngine;
using Dreamers.Gameplay.Pooling;

namespace Dreamers.Gameplay.World
{
    /// <summary>
    /// Track segment featuring collapsed/missing lane sections or crumbling bridge planks.
    /// Requires players to react and switch away from missing lane holes.
    /// </summary>
    public class CollapsingTrackSegment : TrackSegment
    {
        [Header("Lane Holes Configuration")]
        [SerializeField] private GameObject _leftLaneFloor;
        [SerializeField] private GameObject _centerLaneFloor;
        [SerializeField] private GameObject _rightLaneFloor;

        [Header("Hazard Warning Visuals")]
        [SerializeField] private GameObject _hazardTapeLeft;
        [SerializeField] private GameObject _hazardTapeCenter;
        [SerializeField] private GameObject _hazardTapeRight;

        public void ConfigureMissingLanes(bool missingLeft, bool missingCenter, bool missingRight)
        {
            // Guarantee at least one lane is NOT missing
            if (missingLeft && missingCenter && missingRight)
            {
                missingCenter = false;
            }

            if (_leftLaneFloor != null) _leftLaneFloor.SetActive(!missingLeft);
            if (_centerLaneFloor != null) _centerLaneFloor.SetActive(!missingCenter);
            if (_rightLaneFloor != null) _rightLaneFloor.SetActive(!missingRight);

            if (_hazardTapeLeft != null) _hazardTapeLeft.SetActive(missingLeft);
            if (_hazardTapeCenter != null) _hazardTapeCenter.SetActive(missingCenter);
            if (_hazardTapeRight != null) _hazardTapeRight.SetActive(missingRight);
        }
    }
}
