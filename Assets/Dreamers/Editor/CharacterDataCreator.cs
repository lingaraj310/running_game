#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Dreamers.Data;

namespace Dreamers.Editor
{
    /// <summary>
    /// Editor utility to generate baseline ScriptableObject data assets for the 5 initial CSE characters.
    /// Menu: Dreamers > Generate Initial Character Data
    /// </summary>
    public static class CharacterDataCreator
    {
        private const string DATA_FOLDER_PATH = "Assets/Dreamers/Data/Characters";

        [MenuItem("Dreamers/Data/Generate Initial Character Data Assets")]
        public static void GenerateInitialCharacters()
        {
            if (!AssetDatabase.IsValidFolder(DATA_FOLDER_PATH))
            {
                System.IO.Directory.CreateDirectory(DATA_FOLDER_PATH);
                AssetDatabase.Refresh();
            }

            CreateCharacterAsset(
                "SO_Char_Lingaraj",
                "char_lingaraj",
                "Lingaraj",
                "Entrepreneurship",
                "An ambitious visionary passionate about building tech startups and leading engineering ventures.",
                "ability_venture_boost",
                "Venture Boost",
                "Multiplies coin acquisition rate and activates an automated tech node magnet.",
                10.0f,
                0.18f,
                8.5f,
                true,
                0
            );

            CreateCharacterAsset(
                "SO_Char_Bhuvanesh",
                "char_bhuvanesh",
                "Bhuvanesh",
                "Game Development",
                "A creative programmer dedicated to crafting immersive interactive game worlds and physics simulations.",
                "ability_glitch_phase",
                "Glitch Phase",
                "Temporarily phases through static hurdles and obstacles for 3.5 seconds.",
                10.2f,
                0.17f,
                8.7f,
                false,
                1000
            );

            CreateCharacterAsset(
                "SO_Char_Meeha",
                "char_meeha",
                "Meeha",
                "Full-Stack Development",
                "A versatile engineer mastering responsive user interfaces and resilient backend data pipelines.",
                "ability_fullstack_shield",
                "Full-Stack Shield",
                "Deploys an automated stack shield that absorbs one lethal collision without failing the run.",
                9.8f,
                0.18f,
                8.4f,
                false,
                1500
            );

            CreateCharacterAsset(
                "SO_Char_Vedika",
                "char_vedika",
                "Vedika",
                "Cloud Engineering",
                "An infrastructure specialist focused on massive scalability, distributed networks, and cloud resilience.",
                "ability_cloud_surge",
                "Cloud Surge",
                "Reduces jump gravity by 50%, enabling extended airtime and effortless high-lane navigation.",
                10.0f,
                0.18f,
                9.2f,
                false,
                2000
            );

            CreateCharacterAsset(
                "SO_Char_Lakashna",
                "char_lakashna",
                "Lakashna",
                "AI Engineering",
                "A data scientist developing intelligent algorithms, neural models, and predictive systems.",
                "ability_neural_optimizer",
                "Neural Optimizer",
                "Analyzes forward track matrices to visually highlight the optimal, highest-reward lane trajectory.",
                10.4f,
                0.16f,
                8.5f,
                false,
                2500
            );

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[CharacterDataCreator] Successfully generated all 5 character ScriptableObjects in " + DATA_FOLDER_PATH);
        }

        private static void CreateCharacterAsset(
            string assetFileName,
            string id,
            string displayName,
            string theme,
            string description,
            string abilityId,
            string abilityName,
            string abilityDesc,
            float speed,
            float laneSwitchTime,
            float jumpForce,
            bool unlockedByDefault,
            int unlockCost)
        {
            string path = $"{DATA_FOLDER_PATH}/{assetFileName}.asset";
            CharacterDataSO existing = AssetDatabase.LoadAssetAtPath<CharacterDataSO>(path);

            CharacterDataSO asset = existing != null ? existing : ScriptableObject.CreateInstance<CharacterDataSO>();

            SerializedObject serialized = new SerializedObject(asset);
            serialized.FindProperty("_characterId").stringValue = id;
            serialized.FindProperty("_displayName").stringValue = displayName;
            serialized.FindProperty("_careerTheme").stringValue = theme;
            serialized.FindProperty("_description").stringValue = description;
            serialized.FindProperty("_abilityId").stringValue = abilityId;
            serialized.FindProperty("_abilityName").stringValue = abilityName;
            serialized.FindProperty("_abilityDescription").stringValue = abilityDesc;
            serialized.FindProperty("_baseRunSpeed").floatValue = speed;
            serialized.FindProperty("_laneSwitchDuration").floatValue = laneSwitchTime;
            serialized.FindProperty("_jumpForce").floatValue = jumpForce;
            serialized.FindProperty("_isUnlockedByDefault").boolValue = unlockedByDefault;
            serialized.FindProperty("_coinUnlockCost").intValue = unlockCost;
            serialized.ApplyModifiedProperties();

            if (existing == null)
            {
                AssetDatabase.CreateAsset(asset, path);
            }
            else
            {
                EditorUtility.SetDirty(asset);
            }
        }
    }
}
#endif
