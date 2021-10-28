using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script was created to demonstrate api, I do not recommend using it in your projects.
/// </summary>
public class UIControllerDEMO : MonoBehaviour
{

    [Header("I do not recommend using it in your projects")]
    [Header("This script was created to demonstrate api")]
    public CharacterCustomization CharacterCustomization;
    [Space(15)]
    //UI element number
    public Text head_text;
    public Text hair_text;
    public Text skincolor_text;

    public Text hat_text;
    public Text accessory_text;
    public Text shirt_text;
    public Text pant_text;
    public Text shoes_text;

    public Text playbutton_text;

    public Text lod_text;

    public Slider fatSlider;
    public Slider musclesSlider;
    public Slider thinSlider;

    public Slider slimnessSlider;
    public Slider breastSlider;

    public Slider heightSlider;

    #region ButtonEvents

    public void BodyFat()
    {
        CharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.Fat, fatSlider.value);
    }
    public void BodyMuscles()
    {
        CharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.Muscles, musclesSlider.value);
    }
    public void BodyThin()
    {
        CharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.Thin, thinSlider.value);
    }

    public void BodySlimness()
    {
        CharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.Slimness, slimnessSlider.value);
    }
    public void BodyBreast()
    {
        CharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.BreastSize, breastSlider.value, 
            new string[] { "Chest" }, 
            new CharacterCustomization.ClothesPartType[] { CharacterCustomization.ClothesPartType.TShirt }
            );
    }

    public void SetHeight()
    {
        CharacterCustomization.SetHeight(heightSlider.value);
    }

    int lodIndex;
    public void Lod_Event(int next)
    {
        lodIndex += next;
        if (lodIndex < 0)
            lodIndex = 3;
        if(lodIndex > 3)
            lodIndex = 0;

        lod_text.text = lodIndex.ToString();

        CharacterCustomization.ForceLOD(lodIndex);
    }

    public void HeadChange_Event(int next)
    {
        if (next == -1)
            CharacterCustomization.PrevHead();
        else if (next == 1)
            CharacterCustomization.NextHead();

        head_text.text = CharacterCustomization.headActiveIndex.ToString();
    }
    public void HairChange_Event(int next)
    {
        if (next == -1)
            CharacterCustomization.PrevHair();
        else if (next == 1)
            CharacterCustomization.NexHair();


        if (CharacterCustomization.hairActiveIndex == -1)
            hair_text.text = "-";
        else
            hair_text.text = CharacterCustomization.hairActiveIndex.ToString();
    }

    public void HatChange_Event(int next)
    {
        if (next == -1)
            CharacterCustomization.PrevElement(CharacterCustomization.ClothesPartType.Hat);
        else if (next == 1)
            CharacterCustomization.NextElement(CharacterCustomization.ClothesPartType.Hat);

        if (CharacterCustomization.clothesActiveIndexes[CharacterCustomization.ClothesPartType.Hat] == -1)
            hat_text.text = "-";
        else
            hat_text.text = (CharacterCustomization.clothesActiveIndexes[CharacterCustomization.ClothesPartType.Hat] + 1).ToString();
    }

    public void AccessoryChange_Event(int next)
    {
        if (next == -1)
            CharacterCustomization.PrevElement(CharacterCustomization.ClothesPartType.Accessory);
        else if (next == 1)
            CharacterCustomization.NextElement(CharacterCustomization.ClothesPartType.Accessory);

        if (CharacterCustomization.clothesActiveIndexes[CharacterCustomization.ClothesPartType.Accessory] == -1)
            accessory_text.text = "-";
        else
            accessory_text.text = (CharacterCustomization.clothesActiveIndexes[CharacterCustomization.ClothesPartType.Accessory] + 1).ToString();
    }

    public void ShirtChange_Event(int next)
    {
        if (next == -1)
            CharacterCustomization.PrevElement(CharacterCustomization.ClothesPartType.TShirt);
        else if (next == 1)
            CharacterCustomization.NextElement(CharacterCustomization.ClothesPartType.TShirt);

        if (CharacterCustomization.clothesActiveIndexes[CharacterCustomization.ClothesPartType.TShirt] == -1)
            shirt_text.text = "-";
        else
            shirt_text.text = (CharacterCustomization.clothesActiveIndexes[CharacterCustomization.ClothesPartType.TShirt] + 1).ToString();
    }
    public void PantChange_Event(int next)
    {
        if (next == -1)
            CharacterCustomization.PrevElement(CharacterCustomization.ClothesPartType.Pants);
        else if (next == 1)
            CharacterCustomization.NextElement(CharacterCustomization.ClothesPartType.Pants);

        if (CharacterCustomization.clothesActiveIndexes[CharacterCustomization.ClothesPartType.Pants] == -1)
            pant_text.text = "-";
        else
            pant_text.text = (CharacterCustomization.clothesActiveIndexes[CharacterCustomization.ClothesPartType.Pants] + 1).ToString();
    }

    public void ShoesChange_Event(int next)
    {
        if (next == -1)
            CharacterCustomization.PrevElement(CharacterCustomization.ClothesPartType.Shoes);
        else if (next == 1)
            CharacterCustomization.NextElement(CharacterCustomization.ClothesPartType.Shoes);

        if (CharacterCustomization.clothesActiveIndexes[CharacterCustomization.ClothesPartType.Shoes] == -1)
            shoes_text.text = "-";
        else
            shoes_text.text = (CharacterCustomization.clothesActiveIndexes[CharacterCustomization.ClothesPartType.Shoes] + 1).ToString();
    }

    public void SkinChange_Event(int next)
    {
        if (next == -1)
            CharacterCustomization.PrevCharacterMaterial();
        else if (next == 1)
            CharacterCustomization.NextCharacterMaterial();

        skincolor_text.text = CharacterCustomization.materialActiveIndex.ToString();
    }

    bool walk_active = false;
    public void PlayAnim()
    {
        walk_active = !walk_active;

        foreach(Animator a in CharacterCustomization.animators)
            a.SetBool("walk", walk_active);

        playbutton_text.text = (walk_active) ? "STOP" : "PLAY";
    }


    #endregion


    bool canvasVisible = true;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            canvasVisible = !canvasVisible;

            GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>().enabled = canvasVisible;
        }
    }
}
