using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class for customization character (v1.5)
/// </summary>
public class CharacterCustomization : MonoBehaviour
{
    [Header("All character parts")]
    [Header("API Version 1.5")]
    /// <summary>
    /// All character mesh parts
    /// </summary>
    public List<CharacterPart> characterParts = new List<CharacterPart>();
    [Header("Hips for change character height")]
    public List<Transform> originHips = new List<Transform>();
    
    [Header("All clothes anchors")]
    /// <summary>
    /// Anchors for clothes
    /// </summary>
    public List<ClothesAnchor> clothesAnchors = new List<ClothesAnchor>();

    [Header("Array with animators each lod level")]
    //Array with animators each lod level
    public Animator[] animators;
    [Header("Transforms for change character offset by Y coordinate")]
    //Transforms for apply Y offset;
    public Transform[] transforms;
    
    public enum ClothesPartType: int
    {
        Hat,
        TShirt,
        Pants,
        Shoes,
        Accessory
    }

    public enum BodyShapeType : int
    {
        Fat = 0,
        Muscles,Slimness = 1,
        Thin,BreastSize = 2
    }

    //Presets for character customization      
    [Header("All character presets")]
    [Space(15)]
    public List<HeadPreset> headsPresets = new List<HeadPreset>();
    public List<HairPreset> hairPresets = new List<HairPreset>();
    public List<ClothPreset> hatsPresets = new List<ClothPreset>();
    public List<ClothPreset> accessoryPresets = new List<ClothPreset>();
    public List<ClothPreset> shirtsPresets = new List<ClothPreset>();
    public List<ClothPreset> pantsPresets = new List<ClothPreset>();
    public List<ClothPreset> shoesPresets = new List<ClothPreset>();


    [Header("Skin color presets")]
    [Space(15)]
    public List<Material> skinMaterialPresets = new List<Material>();
    public int headActiveIndex { get; private set; } = 0;
    public int hairActiveIndex { get; private set; } = 0;
    public int materialActiveIndex { get; private set; } = 0;

    /// <summary>
    /// All indexes for each cloth
    /// </summary>
    public Dictionary<ClothesPartType, int> clothesActiveIndexes = new Dictionary<ClothesPartType, int>()
    {
        { ClothesPartType.Hat, -1 },
        { ClothesPartType.Accessory, -1 },
        { ClothesPartType.TShirt, -1 },
        { ClothesPartType.Pants, -1 },
        { ClothesPartType.Shoes, -1 },
    };
    private List<float> bodyShapeWeight = new List<float>() { 0f, 0f, 0f };
    LODGroup _lodGroup;

    private void Awake()
    {
        _lodGroup = GetComponent<LODGroup>();
    }

    #region Character elements iterator
    public void NextHead()
    {
        int next = headActiveIndex + 1;
        if (next > headsPresets.Count -1)
            next = 0;
        SetHeadByIndex(next);
    }
    public void PrevHead()
    {
        int next = headActiveIndex - 1;
        if (next < 0)
            next = headsPresets.Count - 1;
        SetHeadByIndex(next);
    }
    public void NexHair()
    {
        int next = hairActiveIndex + 1;
        if (next > hairPresets.Count - 1)
            next = -1;

        SetHairByIndex(next);
    }
    public void PrevHair()
    {
        int next = hairActiveIndex - 1;
        if (next < -1)
            next = hairPresets.Count - 1;

        SetHairByIndex(next);
    }
    public void NextElement(ClothesPartType type)
    {
        int next = clothesActiveIndexes[type] + 1;
        if (next > getPresetArray(type).Count - 1)
            next = -1;
        SetElementByIndex(type, next);
    }
    public void PrevElement(ClothesPartType type)
    {
        int next = clothesActiveIndexes[type] - 1;
        if (next < -1)
            next = getPresetArray(type).Count - 1;
        SetElementByIndex(type, next);
    }
    public void NextCharacterMaterial()
    {
        int next = materialActiveIndex + 1;
        if (next > skinMaterialPresets.Count - 1)
            next = 0;
        SetCharacterMaterialByIndex(next);
    }
    public void PrevCharacterMaterial()
    {
        int next = materialActiveIndex - 1;
        if (next < 0)
            next = skinMaterialPresets.Count - 1;
        SetCharacterMaterialByIndex(next);
    }
    #endregion

    #region Basic functions
    /// <summary>
    /// Set body shape (blend shapes)
    /// </summary>
    /// <param name="type">Body type</param>
    /// <param name="weight">Weight (0-100)</param>
    /// <param name="forPart">Apply to specific body parts</param>
    /// <param name="forClothPart">Apply to specific cloth parts</param>
    public void SetBodyShape(BodyShapeType type, float weight, string[] forPart = null, ClothesPartType[] forClothPart = null)
    {
        foreach(var part in characterParts)
        {
            if (forPart != null && !forPart.Contains(part.name))
                continue;
            foreach (var skinnedMesh in part.skinnedMesh)
            {               
                for(var i = 0;i< skinnedMesh.sharedMesh.blendShapeCount; i++)
                {
                    if(i == (int)type)
                    {
                        skinnedMesh.SetBlendShapeWeight((int)type, weight);
                    }
                }
            }
        }
        foreach(var clothPart in clothesAnchors)
        {
            if (forClothPart != null && !forClothPart.Contains(clothPart.partType))
                continue;

            foreach (var skinnedMesh in clothPart.skinnedMesh)
            {
                if (skinnedMesh.sharedMesh != null)
                {
                    for (var i = 0; i < skinnedMesh.sharedMesh.blendShapeCount; i++)
                    {
                        if (i == (int)type)
                        {
                            skinnedMesh.SetBlendShapeWeight((int)type, weight);
                        }
                    }
                }
            }
        }
        bodyShapeWeight[(int)type] = weight;
    }
    /// <summary>
    /// Change LOD level
    /// </summary>
    /// <param name="lodLevel">LOD level</param>
    public void ForceLOD(int lodLevel)
    {
        if(lodLevel != 0)
        {
            _lodGroup.ForceLOD(lodLevel);
        }
        else
        {
            _lodGroup.ForceLOD(-1);
        }
    }
    /// <summary>
    /// Set character clothes
    /// </summary>
    /// <param name="type">Type of clothes</param>
    /// <param name="index">Index of element</param>
    public void SetElementByIndex(ClothesPartType type, int index)
    {
        ClothesAnchor ca = GetClothesAnchor(type);
        ClothPreset clothPreset = getPreset(type, clothesActiveIndexes[type]);
        float yOffset = 0f;

        if (clothPreset != null)
            UnHideParts(clothPreset.hideParts, type);     

        if(type == ClothesPartType.Pants || type == ClothesPartType.TShirt)
        {
            UnHideParts(new string[] { "Spine" }, type);
        }

        clothesActiveIndexes[type] = index;

        if (index != -1)
        {
            var newPreset = getPreset(type, index);

            yOffset = newPreset.yOffset;

            for (var i = 0; i < ca.skinnedMesh.Length; i++)
            {
                ca.skinnedMesh[i].sharedMesh = newPreset.mesh[i];

                for(var blendCount =0;blendCount < ca.skinnedMesh[i].sharedMesh.blendShapeCount; blendCount++)
                {
                   ca.skinnedMesh[i].SetBlendShapeWeight(blendCount, bodyShapeWeight[blendCount]);
                }               
            }
            ClothPreset shirt = getPreset(ClothesPartType.TShirt, clothesActiveIndexes[ClothesPartType.TShirt]);
            ClothPreset pants = getPreset(ClothesPartType.Pants, clothesActiveIndexes[ClothesPartType.Pants]);
            if((shirt != null && pants != null) && (shirt.notVisibleSpine && pants.notVisibleSpine))
            {
                HideParts(new string[] { "Spine" });
            }
            HideParts(newPreset.hideParts);
        }
        else
        {
            foreach (var sm in ca.skinnedMesh)
                sm.sharedMesh = null;
        }
        if (transforms[0].localPosition.y != yOffset && type == ClothesPartType.Shoes)
        {
            foreach (var tr in transforms)
                tr.localPosition = new Vector3(tr.localPosition.x, yOffset, tr.localPosition.z);
        }
    }
    /// <summary>
    /// Set character head mesh
    /// </summary>
    /// <param name="index"Index of element></param>
    public void SetHeadByIndex(int index)
    {
        CharacterPart head = GetCharacterPart("Head");
        for (var i = 0; i < headsPresets[index].mesh.Length; i++)
        {
            head.skinnedMesh[i].sharedMesh = headsPresets[index].mesh[i];
        }
        headActiveIndex = index;
    }
    /// <summary>
    /// Set body height
    /// </summary>
    /// <param name="height"></param>
    public void SetHeight(float height)
    {
        foreach(var tr in originHips)
        {
            tr.localScale = new Vector3(1+height/2, 1+height, 1+height);
        }
    }
    /// <summary>
    /// Set hair by index
    /// </summary>
    /// <param name="index">index in hair presets</param>
    public void SetHairByIndex(int index)
    {
        CharacterPart hair = GetCharacterPart("Hair");
        if (index != -1)
        {
            for (var i = 0; i < hairPresets[index].mesh.Length; i++)
            {
                hair.skinnedMesh[i].sharedMesh = hairPresets[index].mesh[i];
            }
        }
        else
        {
            foreach(var hairsm in hair.skinnedMesh)
            {
                hairsm.sharedMesh = null;
            }
        }
        hairActiveIndex = index;
    }
    /// <summary>
    /// Get preset array by type
    /// </summary>
    List<ClothPreset> getPresetArray(ClothesPartType type)
    {
        switch (type)
        {
            case ClothesPartType.Hat:
                return hatsPresets;

            case ClothesPartType.TShirt:
                return shirtsPresets;

            case ClothesPartType.Pants:
                return pantsPresets;

            case ClothesPartType.Shoes:
                return shoesPresets;

            case ClothesPartType.Accessory:
                return accessoryPresets;
            default:
                return null;
        }
    }
    /// <summary>
    /// Get preset element
    /// </summary>
    /// <param name="type">Type of clothes</param>
    /// <param name="index">Index</param>
    /// <returns></returns>
    ClothPreset getPreset(ClothesPartType type, int index)
    {
        if (index == -1)
            return null;
        switch (type)
        {
            case ClothesPartType.Hat:
                return hatsPresets[index];

            case ClothesPartType.TShirt:
                return shirtsPresets[index];

            case ClothesPartType.Pants:
                return pantsPresets[index];

            case ClothesPartType.Shoes:
                return shoesPresets[index];

            case ClothesPartType.Accessory:
                return accessoryPresets[index];
            default:
                return null;
        }
    }
    /// <summary>
    /// Get clothes anchor by type
    /// </summary>
    /// <param name="type">Type of clothes</param>
    /// <returns></returns>
    public ClothesAnchor GetClothesAnchor(ClothesPartType type)
    {
        foreach (ClothesAnchor p in clothesAnchors)
        {
            if (p.partType == type)
                return p;
        }
        return null;
    }
    /// <summary>
    /// Get character part by name
    /// </summary>
    /// <param name="name">Part name</param>
    /// <returns></returns>
    public CharacterPart GetCharacterPart(string name)
    {
        foreach(CharacterPart p in characterParts)
        {
            if (p.name == name)
                return p;
        }
        return null;
    }
    /// <summary>
    /// Hide character parts
    /// </summary>
    /// <param name="parts">Array of parts to hide</param>
    public void HideParts(string[] parts)
    {
        foreach (string p in parts)
        {
            foreach (CharacterPart cp in characterParts)
            {
                if (cp.name == p && cp.skinnedMesh[0].enabled)
                {
                    foreach (var mesh in cp.skinnedMesh)
                        mesh.enabled = false;
                }
            }
        }
    }
    /// <summary>
    /// UnHide character parts
    /// </summary>
    /// <param name="parts">Array of parts to unhide</param>
    public void UnHideParts(string[] parts, ClothesPartType hidePartsForElement)
    {
        foreach (string p in parts)
        {
            bool ph_in_shirt = false, ph_in_pants = false, ph_in_shoes = false;

            #region Code to exclude the UnHide parts of the character if are hidden in other presets
            int shirt_i = clothesActiveIndexes[ClothesPartType.TShirt];
            int pants_i = clothesActiveIndexes[ClothesPartType.Pants];
            int shoes_i = clothesActiveIndexes[ClothesPartType.Shoes];

            if (shirt_i != -1 && hidePartsForElement != ClothesPartType.TShirt)
            {
                foreach (var shirtPart in getPreset(ClothesPartType.TShirt, shirt_i).hideParts)
                {
                    if (shirtPart == p)
                    {
                        ph_in_shirt = true;
                        break;
                    }
                }
            }
            if (pants_i != -1 && hidePartsForElement != ClothesPartType.Pants)
            {
                foreach (var pantPart in getPreset(ClothesPartType.Pants, pants_i).hideParts)
                {
                    if (pantPart == p)
                    {
                        ph_in_pants = true;
                        break;
                    }
                }
            }
            if (shoes_i != -1 && hidePartsForElement != ClothesPartType.Shoes)
            {
                foreach (var shoesPart in getPreset(ClothesPartType.Shoes, shoes_i).hideParts)
                {
                    if (shoesPart == p)
                    {
                        ph_in_shoes = true;
                        break;
                    }
                }
            }

            if (ph_in_shirt || ph_in_pants || ph_in_shoes)
                continue;
            #endregion 

            foreach (CharacterPart cp in characterParts)
            {
                if (cp.name == p)
                    foreach (var mesh in cp.skinnedMesh)
                        mesh.enabled = true;
            }
        }
    }
    /// <summary>
    /// Set character material by index
    /// </summary>
    /// <param name="i">Index</param>
    public void SetCharacterMaterialByIndex(int i)
    {
        if (i > skinMaterialPresets.Count - 1 || i < 0)
            return;

        foreach (var cp in characterParts)
        {
            foreach (var mesh in cp.skinnedMesh)
                mesh.material = skinMaterialPresets[i];
        }
        materialActiveIndex = i;
    }
    #endregion

    #region Basic classes
    [System.Serializable]
    public class CharacterPart
    {
        public string name;
        public SkinnedMeshRenderer[] skinnedMesh;
    }
    [System.Serializable]
    public class ClothesAnchor
    {
        public ClothesPartType partType;
        public SkinnedMeshRenderer[] skinnedMesh;
    }
    [System.Serializable]
    public class HeadPreset
    {
        public string name;
        public Mesh[] mesh;
    }
    [System.Serializable]
    public class HairPreset
    {
        public string name;
        public Mesh[] mesh;
    }
    [System.Serializable]
    public class ClothPreset
    {
        public string name;
        public Mesh[] mesh;
        public string[] hideParts;
        public float yOffset = 0;
        public bool notVisibleSpine;
    }
    #endregion
}
