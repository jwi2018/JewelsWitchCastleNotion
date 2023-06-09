using UnityEngine;
using System.Collections;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using System.Collections.Generic;

namespace EnhancedScrollerCustom.StageScroll
{
    public class StageScrollController : MonoBehaviour, IEnhancedScrollerDelegate
    {
        private SmallList<StageScrollData> _data;
        public EnhancedScroller scroller;
        public EnhancedScrollerCellView cellViewPrefab;
        private Dictionary<int, float> _dicRows = new Dictionary<int, float>();
        public RectTransform container = null;

        public RectTransform imageFrameADS = null;
        public RectTransform comingsoon = null;

        public int totalStage;
        public int MyLevel = 1;

        public int numberOfCellsPerRow = 3;

        public List<Sprite> StageImage;
        public List<Sprite> StarImage;

        private void Awake()
        {
            cellViewPrefab.gameObject.SetActive(false);
            imageFrameADS.gameObject.SetActive(false);
            comingsoon.gameObject.SetActive(false);
        }

        private void Start()
        {
            // tell the scroller that this script will be its delegate
            scroller.Delegate = this;

            if (PlayerData.GetInstance != null)
            {
                MyLevel = PlayerData.GetInstance.PresentLevel + 1;
            }

            //50 단위로 맵 업데이트시 comming soon이 나오지 않는 버그때문에 만듬
            int matchstage = StaticGameSettings.TotalStage % 4 == 0 ? 0 : 2;
            
            if (matchstage == 0) totalStage = StaticGameSettings.TotalStage;
            else totalStage = StaticGameSettings.TotalStage + matchstage;

            // totalStage = StaticGameSettings.TotalStage;
            
            LoadData();
        }

        private void OnEnable()
        {
            if (_data != null)
            {
                scroller.ReloadData();
            }
        }
        
        private void LoadData()
        {
            if (_data != null)
            {
                for (var i = 0; i < _data.Count; i++)
                {
                    _data[i].selectedChanged = null;
                }
            }

            int lastLevel = MyLevel;
            //int lastLevel = 17;
            int lastlevelRow = Mathf.CeilToInt((float)lastLevel / (float)numberOfCellsPerRow) - 1;
            if (lastlevelRow < 1)
            {
                lastlevelRow = 0;
            }

            int lastRow = Mathf.CeilToInt((float)totalStage / (float)numberOfCellsPerRow) - 1;

            // set up some simple data
            _data = new SmallList<StageScrollData>();
            for (var i = 0; i < totalStage; i++)
            {
                int row = Mathf.CeilToInt((float)i / (float)numberOfCellsPerRow);

                if (false == _dicRows.ContainsKey(row))
                {
                    if (row == lastlevelRow)
                    {
                        _dicRows.Add(row, 350);
                    }
                    //else if (row == lastRow)
                    //{
                    //    _dicRows.Add(row, 340);
                    //}
                    else
                    {
                        _dicRows.Add(row, 150);
                    }
                }

                _data.Add(new StageScrollData()
                {
                    someText = i.ToString(),
                });
            }

            _dicRows.Add(totalStage, 340);
            _data.Add(new StageScrollData()
            {
                someText = totalStage.ToString(),
            });

            scroller.ReloadData();

            scroller.JumpToDataIndex(lastlevelRow);
        }

        /// <summary>
        /// This function handles the cell view's button click event
        /// </summary>
        /// <param name="cellView">The cell view that had the button clicked</param>
        private void CellViewSelected(StageScrollRowCellView cellView)
        {
            if (cellView == null)
            {
                // nothing was selected
            }
            else
            {
                // get the selected data index of the cell view
                var selectedDataIndex = cellView.DataIndex;

                // loop through each item in the data list and turn
                // on or off the selection state. This is done so that
                // any previous selection states are removed and new
                // ones are added.
                for (var i = 0; i < _data.Count; i++)
                {
                    _data[i].Selected = (selectedDataIndex == i);
                }
            }
        }
        

        #region EnhancedScroller Handlers

        /// <summary>
        /// This tells the scroller the number of cells that should have room allocated.
        /// For this example, the count is the number of data elements divided by the number of cells per row (rounded up using Mathf.CeilToInt)
        /// </summary>
        /// <param name="scroller">The scroller that is requesting the data size</param>
        /// <returns>The number of cells</returns>
        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)_data.Count / (float)numberOfCellsPerRow);
        }

        /// <summary>
        /// This tells the scroller what the size of a given cell will be. Cells can be any size and do not have
        /// to be uniform. For vertical scrollers the cell size will be the height. For horizontal scrollers the
        /// cell size will be the width.
        /// </summary>
        /// <param name="scroller">The scroller requesting the cell size</param>
        /// <param name="dataIndex">The index of the data that the scroller is requesting</param>
        /// <returns>The size of the cell</returns>
        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            //Debug.LogWarningFormat("KKI{0}", dataIndex);
            //return _data[dataIndex].expandedSize;
            if (true == _dicRows.ContainsKey(dataIndex))
            {
                return _dicRows[dataIndex];
            }

            return 180f;
        }

        /// <summary>
        /// Gets the cell to be displayed. You can have numerous cell types, allowing variety in your list.
        /// Some examples of this would be headers, footers, and other grouping cells.
        /// </summary>
        /// <param name="scroller">The scroller requesting the cell</param>
        /// <param name="dataIndex">The index of the data that the scroller is requesting</param>
        /// <param name="cellIndex">The index of the list. This will likely be different from the dataIndex if the scroller is looping</param>
        /// <returns>The cell for the scroller to use</returns>
        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            // first, we get a cell from the scroller by passing a prefab.
            // if the scroller finds one it can recycle it will do so, otherwise
            // it will create a new cell.
            StageScrollCellView cellView = scroller.GetCellView(cellViewPrefab) as StageScrollCellView;

            // data index of the first sub cell
            var di = dataIndex * numberOfCellsPerRow;

            cellView.name = "Cell " + (di).ToString() + " to " + ((di) + numberOfCellsPerRow - 1).ToString();

            // pass in a reference to our data set with the offset for this cell
            cellView.SetData(ref _data, MyLevel, di, this, CellViewSelected);

            if (true == _dicRows.ContainsKey(dataIndex))
            {
                if (_dicRows[dataIndex] == 350)
                {
                    //Debug.LogWarningFormat("KKI {0}", dataIndex);
                    if (null != corAD)
                        StopCoroutine(corAD);
                    corAD = StartCoroutine(ADSRearrange(cellView));
                }
            }

            // return the cell to the scroller
            return cellView;
        }

        private UnityEngine.Coroutine corAD = null;
        private UnityEngine.Coroutine corComingsoon = null;

        private IEnumerator CommingsoonRearrange(StageScrollCellView cellView)
        {
            yield return new WaitForSeconds(0.01f);
        }

        private IEnumerator ADSRearrange(StageScrollCellView cellView)
        {
            yield return new WaitForSeconds(0.01f);

            imageFrameADS.gameObject.SetActive(false);
            imageFrameADS.SetParent(cellView.transform);
            imageFrameADS.anchoredPosition = Vector3.zero;
            imageFrameADS.localScale = Vector3.one;

            imageFrameADS.SetParent(scroller.Container);
            //Debug.LogWarningFormat("KKI  {0} {1}", cellView.name, cellView.GetComponent<RectTransform>().anchoredPosition);
            Vector2 taragetPos = cellView.GetComponent<RectTransform>().anchoredPosition;
            taragetPos.x = 340f;
            taragetPos.y += 90f;
            imageFrameADS.anchoredPosition = taragetPos;
            imageFrameADS.SetSiblingIndex(1);
            imageFrameADS.gameObject.SetActive(true);
        }

        #endregion EnhancedScroller Handlers
    }
}