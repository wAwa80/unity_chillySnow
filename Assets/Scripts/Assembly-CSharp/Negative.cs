using UnityEngine;

[ExecuteInEditMode]
public class Negative : MonoBehaviour
{
	private Material material;

	[SerializeField]
	private Shader shader;

	private void Awake()
	{
		material = new Material(shader);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, material);
	}
}
