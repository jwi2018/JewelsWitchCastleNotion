using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;
using EnhancedUI;
using System;
using Coffee.UIEffects;
using System.Collections.Generic;

namespace EnhancedScrollerCustom.StageScroll
{
    /// <summary>
    /// This is the sub cell of the row cell
    /// </summary>
    public class StageScrollRowCellView : MonoBehaviour
    {
        /// <summary>
        /// These are the UI elements that will be updated when the data changes
        /// </summary>
        public GameObject container;

        public Text text;
        public Image selectionPanel;

        /// <summary>
        /// These are the colors for the selection of the cells
        /// </summary>
        public Color selectedColor;

        public Color unSelectedColor;

        /// <summary>
        /// Public reference to the index of the data
        /// </summary>
        public int DataIndex { get; private set; }

        /// <summary>
        /// The handler to call when this cell's button traps a click event
        /// </summary>
        public SelectedDelegate selected;

        /// <summary>
        /// Reference to the underlying data driving this view
        /// </summary>
        private StageScrollData _data;

        public StageButton _stageButton = null;

        /// <summary>
        /// This is called if the cell is destroyed. The EnhancedScroller will
        /// not call this since it uses recycling, but we include it in case
        /// the user decides to destroy the cell anyway
        /// </summary>
        private void OnDestroy()
        {
            if (_data != null)
            {
                // remove the handler from the data so
                // that any changes to the data won't try
                // to call this destroyed view's function
                _data.selectedChanged -= SelectedChanged;
            }
        }

        /// <summary>
        /// This function just takes the Demo data and displays it
        /// </summary>
        /// <param name="data"></param>
        public void SetData(int dataIndex, StageScrollData data, int myLevel, StageScrollController controller, SelectedDelegate selected)
        {
            // set the selected delegate
            this.selected = selected;

            // this cell was outside the range of the data, so we disable the container.
            // Note: We could have disable the cell gameobject instead of a child container,
            // but that can cause problems if you are trying to get components (disabled objects are ignored).
            container.SetActive(data != null);

            if (data != null)
            {
                int index = dataIndex + 1;
                // set the text if the cell is inside the data range
                _stageButton._getStageNum = index;
                if (index < myLevel)
                {
                    _stageButton.GetComponent<Image>().sprite = controller.StageImage[2];

                    int StarCnt = 0;
                    if (PlayerData.GetInstance != null)
                    {
                        StarCnt = PlayerData.GetInstance.GetLevelStartCount(index);
                        StarCnt = Mathf.Max(StarCnt, 1);
                    }

                    _stageButton.SetStars = controller.StarImage[StarCnt - 1];
                    List<Color> colors = _stageButton.GetTextColors(0);
                    _stageButton.GetText.color = colors[0];
                    _stageButton.GetShadow.effectColor = colors[1];
                    _stageButton.GetOutline.effectColor = colors[2];

                    UIShiny shiny = _stageButton.GetComponent<UIShiny>();
                    if (shiny != null)
                    {
                        Destroy(shiny);
                    }
                }
                else if (_stageButton._getStageNum == myLevel)
                {
                    _stageButton.GetComponent<Image>().sprite = controller.StageImage[1];
                    _stageButton.SetStars = null;
                    List<Color> colors = _stageButton.GetTextColors(1);
                    _stageButton.GetText.color = colors[0];
                    _stageButton.GetShadow.effectColor = colors[1];
                    _stageButton.GetOutline.effectColor = colors[2];

                    UIShiny shiny = _stageButton.GetComponent<UIShiny>();
                    if (shiny == null)
                    {
                        shiny = _stageButton.gameObject.AddComponent<UIShiny>();
                        shiny.effectPlayer.loop = true;
                        shiny.width = 1;
                        shiny.rotation = -30f;
                        shiny.brightness = 0.255f;
                        shiny.effectArea = EffectArea.Character;
                        shiny.Play(true);
                    }
                }
                else
                {
                    _stageButton.SetStars = null;
                    _stageButton.GetComponent<Image>().sprite = controller.StageImage[0];
                    List<Color> colors = _stageButton.GetTextColors(2);
                    _stageButton.GetText.color = colors[0];
                    _stageButton.GetShadow.effectColor = colors[1];
                    _stageButton.GetOutline.effectColor = colors[2];

                    UIShiny shiny = _stageButton.GetComponent<UIShiny>();
                    if (shiny != null)
                    {
                        Destroy(shiny);
                    }
                }

                text.text = data.someText;

                SetPlusRewardBox(index, myLevel);
            }

            // if there was previous data assigned to this cell view,
            // we need to remove the handler for the selection change
            if (_data != null)
            {
                _data.selectedChanged -= SelectedChanged;
            }

            // link the data to the cell view
            DataIndex = dataIndex;
            _data = data;

            if (data != null)
            {
                // set up a handler so that when the data changes
                // the cell view will update accordingly. We only
                // want a single handler for this cell view, so
                // first we remove any previous handlers before
                // adding the new one
                _data.selectedChanged -= SelectedChanged;
                _data.selectedChanged += SelectedChanged;

                // update the selection state UI
                SelectedChanged(data.Selected);
            }
        }

        private void SetPlusRewardBox(int index, int mylevel)
        {
            if (index % 15 == 0 && index >= mylevel)
            {
                _stageButton.SetRewardBox.gameObject.SetActiveSelf(true);
            }
            else
            {
                _stageButton.SetRewardBox.gameObject.SetActiveSelf(false);
            }
        }

        /// <summary>
        /// This function changes the UI state when the item is
        /// selected or unselected.
        /// </summary>
        /// <param name="selected">The selection state of the cell</param>
        private void SelectedChanged(bool selected)
        {
            selectionPanel.color = (selected ? selectedColor : unSelectedColor);
        }

        /// <summary>
        /// This function is called by the cell's button click event
        /// </summary>
        public void OnSelected()
        {
            if (StaticGameSettings.StageOpenDebug == false && DataIndex > PlayerData.GetInstance.PresentLevel)
            {
                return;
            }
            var obj = GameObject.Find("PopupManager");
            obj.GetComponent<PopupManager>().InputStageNumber(DataIndex + 1);
            obj.GetComponent<PopupManager>().CallLoadingTutorialPop("GameScene");
            SoundManager.GetInstance.Play("ButtonPush");
            if (selected != null) selected(this);
        }
    }
}