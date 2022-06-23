using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Figure selectedFigure, lastSelectableFigure;
    [SerializeField]
    List<Figure> stalableFigures = new List<Figure>();
    private bool selectedColided;
    [SerializeField]
    private Material collEnterMat, selectableMat;
    [SerializeField]
    private float loseY;
    [SerializeField]
    private float checkStabilityTime;
    private float currentStabilityTime = 0f;
    [SerializeField]
    private bool isCheckStability;
    [SerializeField]
    private GameObject stabilityPanel,losePanel,winPanel;
    [SerializeField]
    private Image imageStabilityBand;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !selectedColided && !isCheckStability)
        {
            if (selectedFigure == null)
            {
                RaycastHit hit = CastRay();

                if (hit.transform!=null && !hit.transform.gameObject.GetComponentInParent<Figure>().IsSelectableObject)
                {
                    return;
                }
                else if (hit.transform.gameObject.GetComponentInParent<Figure>())
                {

                    Figure figure = hit.transform.gameObject.GetComponentInParent<Figure>();
                    selectedFigure = figure;
                    selectedFigure.DisableObject();
                    selectedFigure.ChangeRendereMat(selectableMat);
                    stalableFigures.Add(selectedFigure);
                    Cursor.visible = false;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && !selectedColided)
        {
            if (selectedFigure != null)
            {
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedFigure.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                selectedFigure.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0f);

                selectedFigure.ActiavateObject();
                selectedFigure.ChangeSelectable(false);
                selectedFigure.ChangeRendereMat(selectedFigure.FigureMaterial);

                lastSelectableFigure = selectedFigure;
                selectedFigure = null;
                isCheckStability = true;
            }
        }
        if (selectedFigure != null && Input.GetMouseButton(0))
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedFigure.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            worldPosition.z = Mathf.Clamp(worldPosition.z,-3f,0f);
            selectedFigure.transform.position = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);

            selectedColided = selectedFigure.IsCollEnter();
            if (selectedColided)
            {
                selectedFigure.ChangeRendereMat(collEnterMat);
            }
            else
            {
                selectedFigure.ChangeRendereMat(selectableMat);
            }
        }
        if (isCheckStability && lastSelectableFigure!=null)
        {
            if (currentStabilityTime >= checkStabilityTime)
            {
                stabilityPanel.SetActive(false);
                currentStabilityTime = 0f;
                isCheckStability = false;
                lastSelectableFigure = null;
                Cursor.visible = true;

                if (stalableFigures.Count >= 3)
                {
                    ActivateWinPanel();
                }
                Debug.Log("StabilityTrue");
            }
            else if(lastSelectableFigure.IsStabilityFiguree())
            {
                stabilityPanel.SetActive(true);
                currentStabilityTime += Time.deltaTime;
                imageStabilityBand.fillAmount = currentStabilityTime / checkStabilityTime;
                Debug.Log("StabilityCheck");
            }

            CheckLose();
        }
    }

    private void CheckLose()
    {
        foreach (var figure in stalableFigures)
        {
            if (figure.transform.position.y <= loseY)
            {
                AcitavateLosePanel();
                Cursor.visible = true;
            }
        }
    }
    private void ActivateWinPanel()
    {
        winPanel.SetActive(true);
        losePanel.SetActive(false);
        stabilityPanel.SetActive(false);
    }
    private void AcitavateLosePanel()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(true);
        stabilityPanel.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane
            );
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);
        return hit;
    }
}
