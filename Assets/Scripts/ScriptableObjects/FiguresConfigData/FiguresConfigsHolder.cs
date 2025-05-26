using UnityEngine;


[CreateAssetMenu(fileName = "FiguresConfigsHolder", menuName = "CustomSO/FiguresConfigsHolder", order = 0)]
public class FiguresConfigsHolder : ScriptableObject
{
    [SerializeField] private FigureConfig[] figureConfigs;

    public FigureConfig GetConfig (FiguresTypes type)
    {
        for(int i = 0; i < figureConfigs.Length; i++)
        {
            if (figureConfigs[i].FigureType == type)
            {
                return figureConfigs[i];
            }
        }

        return null;
    }
}
