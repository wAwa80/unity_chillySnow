using UnityEngine;

[ExecuteInEditMode]
public class Negative : MonoBehaviour
{
	private Material material;

	[SerializeField]
	private Shader shader;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		try
		{
			if (material == null)
			{
				material = new Material(shader);
			}
			Graphics.Blit(source, destination, material);
		}
		catch
		{
		}
	}
}
