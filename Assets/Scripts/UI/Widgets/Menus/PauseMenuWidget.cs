using UnityEngine;
using System;
using TMPro;

[System.Serializable]
public enum PauseMenuMainTabs
{
    Journal, Equipment, Stats, Abilities, Inventory, System
}

[System.Serializable]
public enum PauseMenuJournalTabs
{
    Maps, Enemies, Locations, Groups, Hints
}

[System.Serializable]
public enum PauseMenuInventoryTabs
{
    Armor, Weapons, Douments, KeyItems
}

public class PauseMenuWidget : UIMenu
{
    [Header("General")]
    [SerializeField] UIWidgetData optionsMenuWidgetData;
    [SerializeField] private GameObject itemListEntryPrefab;
    
    [Header("Journal")]
    [SerializeField] private GameObject journalPage;
    [SerializeField] private GameObject journalListContent;
    [SerializeField] private TextMeshProUGUI description;
    
    [Header("Equipment")]
    [SerializeField] private GameObject equipmentPage;

    [Header("Stats")]
    [SerializeField] private GameObject statsPage;

    [Header("Abilities")]
    [SerializeField] private GameObject abilitiesPage;

    [Header("Inventory")]
    [SerializeField] private GameObject inventoryPage;

    [Header("System")]
    [SerializeField] private GameObject systemPage;
    [SerializeField] private SceneData mainMenuScene;
    [SerializeField] SceneChannel requestChangeSceneChannel;
    [SerializeField] UIWidgetDataChannel requestLoadUIWidgetChannel;
    [SerializeField] UIWidgetDataChannel requestUnLoadUIWidgetChannel;

    public override void Setup(UIWidgetData newUIWidgetData)
    {
        base.Setup(newUIWidgetData);
    }

    public override void Teardown()
    {
        base.Teardown();
    }

    public void OnClick_Main_Tab(int tab)
    {
        
        StartCoroutine(Delay(Main_TabButton, tab, 0.1f));
    }

    private void Main_TabButton(int _tab)
    {
        if(_tab > Enum.GetNames(typeof(PauseMenuMainTabs)).Length)
        {
            Debug.LogWarning("PauseMenuWidget: MainTabButton() recieved a tab value out of range");
            return;
        }
        
        PauseMenuMainTabs tab = (PauseMenuMainTabs) _tab;

        // Deactivate all tabs
        journalPage.SetActive(false);
        equipmentPage.SetActive(false);
        statsPage.SetActive(false);
        abilitiesPage.SetActive(false);
        inventoryPage.SetActive(false);
        systemPage.SetActive(false);

        // Activate the chosen tab
        switch (tab)
        {
            case PauseMenuMainTabs.Journal:
                journalPage.SetActive(true);
                break;
            case PauseMenuMainTabs.Equipment:
                equipmentPage.SetActive(true);
                break;
            case PauseMenuMainTabs.Stats:
                statsPage.SetActive(true);
                break;
            case PauseMenuMainTabs.Abilities:
                abilitiesPage.SetActive(true);
                break;
            case PauseMenuMainTabs.Inventory:
                inventoryPage.SetActive(true);
                break;
            case PauseMenuMainTabs.System:
                systemPage.SetActive(true);
                break;
            default:
                break;
        }
    }

    // Journal ============================

    public void OnClick_Journal_Tab(int tab)
    {   
        StartCoroutine(Delay(Journal_TabButton, tab, 0.1f));
    }

    private void Journal_TabButton(int _tab)
    {
        if(_tab > Enum.GetNames(typeof(PauseMenuJournalTabs)).Length)
        {
            Debug.LogWarning("PauseMenuWidget: JournalTabButton() recieved a tab value out of range");
            return;
        }
        
        PauseMenuJournalTabs tab = (PauseMenuJournalTabs) _tab;
    }

    public void OnClick_Journal_Entry()
    {

    }

    // Equipment ============================

    public void OnClick_Equipment_Tab(int partyIndex)
    {
        StartCoroutine(Delay(Equipment_TabButton, partyIndex, 0.1f));
    }

    private void Equipment_TabButton(int partyIndex)
    {

    }

    public void OnClick_Equipment_Slot()
    {

    }

    public void OnClick_Equipment_NewGear()
    {

    }

    // Stats ============================

    public void OnClick_Stats_Tab(int partyIndex)
    {
        StartCoroutine(Delay(Stats_TabButton, partyIndex, 0.1f));
    }

    private void Stats_TabButton(int partyIndex)
    {

    }

    // Abilities ============================

    public void OnClick_Abilities_Tab(int partyIndex)
    {
        StartCoroutine(Delay(Abilities_TabButton, partyIndex, 0.1f));
    }

    private void Abilities_TabButton(int partyIndex)
    {

    }

    public void OnClick_Abilities_Entry()
    {

    }

    // Inventory ============================

    public void OnClick_Inventory_Tab(int tab)
    {   
        StartCoroutine(Delay(Inventory_TabButton, tab, 0.1f));
    }

    private void Inventory_TabButton(int _tab)
    {
        if(_tab > Enum.GetNames(typeof(PauseMenuInventoryTabs)).Length)
        {
            Debug.LogWarning("PauseMenuWidget: JournalTabButton() recieved a tab value out of range");
            return;
        }

        PauseMenuInventoryTabs tab = (PauseMenuInventoryTabs) _tab;
    }

    public void OnClick_Inventory_Entry()
    {

    }

    // System ============================

    public void OnClick_System_Resume()
    {
        StartCoroutine(Delay(System_ResumeButton, 0.1f));
    }

    private void System_ResumeButton()
    {
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }

    public void OnClick_System_Options()
    {
        StartCoroutine(Delay(System_OptionsButton, 0.1f));
    }

    private void System_OptionsButton()
    {
        requestLoadUIWidgetChannel.Raise(optionsMenuWidgetData);
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }

    public void OnClick_System_Save()
    {
        StartCoroutine(Delay(System_SaveButton, 0.1f));
    }

    public void System_SaveButton()
    {

    }

    public void OnClick_System_Load()
    {
        StartCoroutine(Delay(System_LoadButton, 0.1f));
    }

    public void System_LoadButton()
    {

    }

    public void OnClick_System_Quit()
    {
        StartCoroutine(Delay(System_QuitButton, 0.1f));
    }

    private void System_QuitButton()
    {
        requestChangeSceneChannel.Raise(mainMenuScene);
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }
}
