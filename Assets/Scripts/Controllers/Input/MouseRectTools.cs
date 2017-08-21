using UnityEngine;

public static class MouseRectTools {
    static Texture2D _whiteTexture;
    public static Texture2D WhiteTexture
    {
        get
        {
            if( _whiteTexture == null )
            {
                _whiteTexture = new Texture2D( 1, 1 );
                _whiteTexture.SetPixel( 0, 0, Color.white );
                _whiteTexture.Apply();
            }

            return _whiteTexture;
        }
    }

    public static void DrawScreenRect( Rect rect, Color color )//рисуем прямоугольник
    {
        GUI.color = color;
        GUI.DrawTexture( rect, WhiteTexture );
        GUI.color = Color.white;
    }

    public static void DrawScreenRectBorder( Rect rect, float thickness, Color color )//рисуем границы прямоугольника
    {
        // верх
        MouseRectTools.DrawScreenRect( new Rect( rect.xMin, rect.yMin, rect.width, thickness ), color );
        // лево
        MouseRectTools.DrawScreenRect( new Rect( rect.xMin, rect.yMin, thickness, rect.height ), color );
        // право
        MouseRectTools.DrawScreenRect( new Rect( rect.xMax - thickness, rect.yMin, thickness, rect.height ), color);
        // низ
        MouseRectTools.DrawScreenRect( new Rect( rect.xMin, rect.yMax - thickness, rect.width, thickness ), color );
    }

    public static Rect GetScreenRect( Vector3 screenPosition1, Vector3 screenPosition2 )
    {
        // Перемещение координат из левого нижнего в левый верхний угол
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Рассчитать углы
        var topLeft = Vector3.Min( screenPosition1, screenPosition2 );
        var bottomRight = Vector3.Max( screenPosition1, screenPosition2 );
        // Создать прямоугольник
        return Rect.MinMaxRect( topLeft.x, topLeft.y, bottomRight.x, bottomRight.y );
    }

    public static Bounds GetViewportBounds( Camera camera, Vector3 screenPosition1, Vector3 screenPosition2 )
    {
        Vector3 v1 = Camera.main.ScreenToViewportPoint( screenPosition1 );
        Vector3 v2 = Camera.main.ScreenToViewportPoint( screenPosition2 );
        Vector3 min = Vector3.Min( v1, v2 );
        Vector3 max = Vector3.Max( v1, v2 );
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        Bounds bounds = new Bounds();
        bounds.SetMinMax( min, max );
        return bounds;
    }

}