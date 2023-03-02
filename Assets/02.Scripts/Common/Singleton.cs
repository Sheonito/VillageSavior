/*
작성자: 최재호(cjh0798@gmail.com)
기능: MonoBehaviour 이외 싱글톤
 */
public class Singleton<T> where T : new() // T를 new로 생성하기 위해 T를 new()로 제한
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
