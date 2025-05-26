using System.Collections.Generic;

public class FiguresOnFieldHolder: IFiguresOnFieldHolder, IAddebleModelFiguresHolder, IClearableFiguresOnFieldHolder
{
    private GameConfig _gameConfig;
    private List<FiguresCollectionModel> _figuresCollectionModels;

    public FiguresOnFieldHolder(GameConfig gameConfig)
    {
        _gameConfig = gameConfig;
        _figuresCollectionModels = new List<FiguresCollectionModel>(gameConfig.FiguresTypesCountOnField);
    }

    public void AddModel(FiguresTypes type, FigureView view)
    {
        for(int i = 0; i < _figuresCollectionModels.Count; i++)
        {
            if (_figuresCollectionModels[i].FiguresType == type)
            {
                _figuresCollectionModels[i].AddView(view);
                return;
            }
        }

        var collectionModel = new FiguresCollectionModel(type, _gameConfig.FiguresCollectionLenth);

        collectionModel.AddView(view);
        _figuresCollectionModels.Add(collectionModel);
    }

    public FiguresCollectionModel GetCollectionByType(FiguresTypes type)
    {
        for(int i = 0; i < _figuresCollectionModels.Count; i++)
        {
            if (_figuresCollectionModels[i].FiguresType == type)
                return _figuresCollectionModels[i];
        }

        return null;
    }

    public void RemoveFiguresCollection(FiguresCollectionModel figuresCollectionModel)
    {
        figuresCollectionModel.ClearCollection();
        _figuresCollectionModels.Remove(figuresCollectionModel);
    }

    public bool IsNoFiguresOnField() => _figuresCollectionModels.Count == 0;

    public int GetCollectionsCount() => _figuresCollectionModels.Count;

    List<FiguresCollectionModel> IClearableFiguresOnFieldHolder.GetCollectionsForClear() => _figuresCollectionModels;
}