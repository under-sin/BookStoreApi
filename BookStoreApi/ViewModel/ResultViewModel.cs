namespace BookStoreApi.ViewModel;

public class ResultViewModel<T>
{
    // O result vai ter vários construtores para padronizar o retorno da api
    public ResultViewModel(T data, List<string> errors)
    {
        Data = data;
        Errors = errors;
    }

    public ResultViewModel(T data)
    {
        Data = data;
    }

    public ResultViewModel(List<string> errors)
    {
        Errors = errors;
    }

    public ResultViewModel(string error)
    {
        Errors.Add(error);
    }

    public T Data { get; private set; }
    public List<string> Errors { get; private set; } = new();
}
