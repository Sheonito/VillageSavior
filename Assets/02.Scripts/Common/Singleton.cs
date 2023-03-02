/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: MonoBehaviour �̿� �̱���
 */
public class Singleton<T> where T : new() // T�� new�� �����ϱ� ���� T�� new()�� ����
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = new T();

            return instance;
        }
        private set { }
    }
}
