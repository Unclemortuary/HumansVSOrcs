using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour {
	bool isSelecting = false;
	Vector3 mousePosition1;

	void Update()
	{
		MouseCheck ();
	}

	void MouseCheck()
	{
		// Если нажимаем на левую кнопку мыши, то
		// сохраняем координаты курсора мыши и начинаем выбор
		if( Input.GetMouseButtonDown(0))
		{
			isSelecting = true;
			mousePosition1 = Input.mousePosition;
		}
		// Если мы отпускаем левую кнопку мыши - конец выбора
		if (Input.GetMouseButtonUp (0)) {
			isSelecting = false;
		}
	}

	public bool IsWithinSelectionBounds( GameObject gameObject )
	{
		if( !isSelecting )
			return false;

		var camera = Camera.main;
		var viewportBounds = MouseRect.GetViewportBounds( camera, mousePosition1, Input.mousePosition );

		return viewportBounds.Contains(camera.WorldToViewportPoint( gameObject.transform.position ));
	}

	void OnGUI()
	{
		if(isSelecting)
		{
				// Создаем прямоугольник на основе начальных и конечных координат курсора
				var rect = MouseRect.GetScreenRect( mousePosition1, Input.mousePosition );
				MouseRect.DrawScreenRect( rect, new Color( 0.8f, 0.8f, 0.95f, 0.25f ) );
				MouseRect.DrawScreenRectBorder( rect, 2, new Color( 0.8f, 0.8f, 0.95f ) );
		}
	}
}
