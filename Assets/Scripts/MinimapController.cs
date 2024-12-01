using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public GameObject PrefabIslandIcon;
    public GameObject PlayerIcon;
    private List<IslandIconController> ListIslandIcon;

    void Start()
    {
        var arrayIslandIcons = GetComponentsInChildren<IslandIconController>();
        ListIslandIcon = arrayIslandIcons.ToList();
    }

    public void CreateNewIslandIcon()
    {
        var islandIcon = Instantiate(PrefabIslandIcon.gameObject, transform);
        ListIslandIcon.Add(islandIcon.GetComponent<IslandIconController>());
    }

    public void UpdateListIslandIcon(List<Vector3> listPosition)
    {
        for (var i = 0; i < listPosition.Count; i++)
        {
            var islandIcon = ListIslandIcon[i];
            islandIcon.transform.localPosition = ConvertWorldToMinimapPos(listPosition[i]);
        }
    }


    public void RemoveIslandIconAt(int idx)
    {
        if (idx < 0)
        {
            return;
        }
        
        var icon = ListIslandIcon[idx];
        Destroy(icon.gameObject);
        ListIslandIcon.RemoveAt(idx);
    }

    public void UpdatePlayerIcon(Vector3 position)
    {
        this.PlayerIcon.transform.localPosition = ConvertWorldToMinimapPos(position);
    }

    private Vector3 ConvertWorldToMinimapPos(Vector3 position)
    {
        var w = this.GetComponent<RectTransform>().rect.width;
        var h = this.GetComponent<RectTransform>().rect.height;
        var posX = position.x;
        var posY = position.y;
        var realMapWidth = 20f;
        var realMapHeight = 12f;

        var minimapX = (w / 2f) * (posX / (realMapWidth / 2f));
        var minimapY = (h / 2f) * (posY / (realMapHeight / 2f));

        return new Vector3(minimapX, minimapY, position.z);
    }
}