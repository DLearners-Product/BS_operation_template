# Blended Integration SOP

**Note : Before Integration all Development, bugfixes and changes should be done.**

Clone Blended Template using following command:

``
git submodule add https://github.com/DLearners-Product/BS_operation_template.git Assets/BlendedTemplate/
``

make sure project in following folder structre:

Assets/
  - BlendedTemplate/

## Script Attachments :
  - Attach `BlendedOperation.cs` and `Bridge.cs` to Same GO in which `Main_Blended.cs` is attached.
  - Create Empty GO Scipt and create follwoing Gamobject and attach script
    - MainBlendedData.cs -> **MainBlendedData GO**
    - ActivityContentManager.cs -> **ActivityContentManager GO**
    - QAManager.cs -> **QAManager GO**
    - ScoreManager.cs -> **ScoreManager GO**
 - Create an empty GameObject named ***Prefab*** and place all slide prefabs under it

## Drag and Drops :
### MainBlendedData.cs
  - Create `SlideDatas == Count(Activity)`
  - Drag and drop all slide prefabs under `SlideObject` in `SlideDatas`
  - And check `TextComponent` object populated in `TextComponents` object those component will be used for (remove if not needed):
    - Syllabification.
    - Text data storage in back-end (Can be chnaged from back-end if needed).
  - Enter `Slide Name`
  - Emter `Teacher Instruction`
  - Enter `Activity Instruction`
  - Mark ``bool``s accordingly if that slide has any of it.
### ActivityContentManager.cskkkf
This script contains all activity Questions, Options and datatypes.
  - Put all question and option assets in Activity object.

## Script Changes
### QAManager.cs
   - Map question and option using keyword
   - Add Regions and default functions

### BlendedOptions.cs
  - Call `NotifyActivityCompleted()` on activity completion condition.

### Main_Blended.cs
  - At `THI_cloneLevels()` change the slide instantiate method to
 
      ```var currentLevel = Instantiate(MainBlendedData.instance.slideDatas[levelno].slideObject);```
  - Change B_pause to access modifier public.

## Component Changes :
### VideoPlayer Component
  - Import Video Loader package
  - Map `LoadingPanel` prefab to *VidoeLoading* object in `VideoProgressbar` Scirpt.
  - Disable `Play on Awake` in Video Player Component.
