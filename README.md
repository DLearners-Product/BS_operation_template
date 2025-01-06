# Blended Integration SOP

>[!NOTE]  
>**Before Integration all Development, bugfixes and changes should be done.**

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
### ActivityContentManager.cs
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

# Push integration to repository:

To push BlendedTemplate to repository
  - Go to Assets/BlendedTemplate/.
  - In that folder change `.gitignore` from *.meta -> # *.meta and save the file.
  - Now you will be able to see meta file UnStaged in git.
  - And then checkout to new branch in the `name of main repository`
    - Eg. If you're working in lesson name `R Blends` then consider repo name `BS_R_Blends` then branch should be named as `BS_R_Blends`.
  - Then push those meta file to repo in branch named with main repo name.
  - Now these changes will get reflected in main repository push those changes to **DESIGNATED BRANCH**.

# How to clone (if integration done) :

>[!IMPORTANT]  
>To Perform this action kindly ensure [Push integration to repository](#push-integration-to-repository) is done.

  - Clone repository switch to **DESIGNATED BRANCH** in which integration is done and pushed.
  - And then run following command in root of the project.  

    ``git submodule update --init --recursive``

  - All the integration changes will get reflected along with meta files.
  - If needed you can navigate to `Assets/BlendedTemplate/` directory and switch to branch which named in main repo.