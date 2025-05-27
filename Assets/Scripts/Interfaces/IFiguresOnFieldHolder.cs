public interface IFiguresOnFieldHolder
{
    public FiguresCollectionModel GetCollectionModelByType(FiguresTypes type);
    public void RemoveFiguresCollection(FiguresCollectionModel figuresCollectionModel);
    public bool IsNoFiguresOnField();
    public int GetCollectionsCount();

    public FiguresTypes[] GetCollectionsTypes();
}
