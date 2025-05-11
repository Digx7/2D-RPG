using UnityEngine;
using System;

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
    [SerializeField] UIWidgetData optionsMenuWidgetData;
    
    [Header("System")]
    [SerializeField] SceneData mainMenuScene;
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

    public void OnClickMainTab(PauseMenuMainTabs tab)
    {
        int _tab = (int) tab;
        
        StartCoroutine(Delay(MainTabButton, _tab, 0.1f));
    }

    private void MainTabButton(int _tab)
    {
        PauseMenuMainTabs tab = (PauseMenuMainTabs) _tab;
    }

    // Journal ============================

    public void OnClickJournalTab(PauseMenuJournalTabs tab)
    {
        int _tab = (int) tab;
        
        StartCoroutine(Delay(JournalTabButton, _tab, 0.1f));
    }

    private void JournalTabButton(int _tab)
    {
        PauseMenuInventoryTabs tab = (PauseMenuInventoryTabs) _tab;
    }

    // Equipment ============================

    public void OnClickEquipmentTab(int partyIndex)
    {
        StartCoroutine(Delay(EquipmentTabButton, partyIndex, 0.1f));
    }

    private void EquipmentTabButton(int partyIndex)
    {

    }

    // Stats ============================

    public void OnClickStatsTab(int partyIndex)
    {
        StartCoroutine(Delay(StatsTabButton, partyIndex, 0.1f));
    }

    private void StatsTabButton(int partyIndex)
    {

    }

    // Abilities ============================

    public void OnClickAbilitiesTab(int partyIndex)
    {
        StartCoroutine(Delay(AbilitiesTabButton, partyIndex, 0.1f));
    }

    private void AbilitiesTabButton(int partyIndex)
    {

    }

    // Inventory ============================

    public void OnClickInventoryTab(PauseMenuInventoryTabs tab)
    {
        int _tab = (int) tab;
        
        StartCoroutine(Delay(InventoryTabButton, _tab, 0.1f));
    }

    private void InventoryTabButton(int tab)
    {

    }

    // System ============================

    public void OnClickResume()
    {
        StartCoroutine(Delay(ResumeButton, 0.1f));
    }

    private void ResumeButton()
    {
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }

    public void OnClickOptions()
    {
        StartCoroutine(Delay(OptionsButton, 0.1f));
    }

    private void OptionsButton()
    {
        requestLoadUIWidgetChannel.Raise(optionsMenuWidgetData);
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }

    public void OnClickQuit()
    {
        StartCoroutine(Delay(QuitButton, 0.1f));
    }

    private void QuitButton()
    {
        requestChangeSceneChannel.Raise(mainMenuScene);
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }
}
