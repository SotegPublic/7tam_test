public interface IFiguresOnFieldHolder
{
    public FiguresCollectionModel GetCollectionByType(FiguresTypes type);
    public void RemoveFiguresCollection(FiguresCollectionModel figuresCollectionModel);
    public bool IsNoFiguresOnField();
    public int GetCollectionsCount();
}
