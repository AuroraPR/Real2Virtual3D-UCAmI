using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class control2 : MonoBehaviour
{
    public GameObject spherePrefab;
    public GameObject linePrefab;

    private List<GameObject> spheres = new List<GameObject>();
    private List<GameObject> lines = new List<GameObject>();

    public Color sphereColor; // Color de las esferas
    public Color headColor; // Color de la cabeza (primera esfera)

    void Start()
    {
        StartCoroutine(CreateSpheresFromFiles());
    }

    IEnumerator CreateSpheresFromFiles()
    {
        string[] files = System.IO.Directory.GetFiles("Assets/auro", "*.json");

        foreach (string file in files)
        {
            ClearSpheresAndLines();

            string[] lines = System.IO.File.ReadAllLines(file);

            // Crear esferas
            CreateSpheres(lines.Length);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] coordinates = lines[i].Replace('.', ',').Split('\t');

                float x = (float.Parse(coordinates[0]) * 10f)+3f;
                float y = (1-float.Parse(coordinates[1])) * 10f+3f;
                float z = float.Parse(coordinates[2]) * 10f+3f;

                SetSpherePosition(i, new Vector3(x, y, z));

                // Ajustar tamaño de la primera esfera (cabeza)
                if (i == 0)
                {
                    SetSphereScale(i, new Vector3(1.5f, 1.5f, 1.5f)); // Ajusta la escala según tus necesidades
                    SetSphereColor(i, headColor); // Establece el color de la cabeza
                }
                else
                {
                    SetSphereColor(i, sphereColor); // Establece el color de las esferas
                }
            }

            // Establecer uniones entre las esferas según el patrón
            ConnectSpheres();

            yield return new WaitForSeconds(1f);
        }
    }

    void CreateSpheres(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject sphere = Instantiate(spherePrefab, Vector3.zero, Quaternion.identity);
            sphere.transform.localScale = new Vector3(1f, 1f, 1f);
            spheres.Add(sphere);
        }
    }

    void SetSpherePosition(int index, Vector3 position)
    {
        GameObject sphere = spheres[index];
        sphere.transform.position = position;
    }

    void SetSphereScale(int index, Vector3 scale)
    {
        GameObject sphere = spheres[index];
        sphere.transform.localScale = scale;
    }

    void SetSphereColor(int index, Color color)
    {
        GameObject sphere = spheres[index];
        Renderer renderer = sphere.GetComponent<Renderer>();
        renderer.material.color = color;
    }

    void ConnectSpheres()
    {
        // Uniones para esfera 0
        ConnectSpheresByIndices(0, new int[] { 1, 2 });

        // Uniones para esfera 1
        ConnectSpheresByIndices(1, new int[] { 2, 3, 7 });

        // Uniones para esfera 2
        ConnectSpheresByIndices(2, new int[] { 4, 8 });

        // Uniones para esfera 3
        ConnectSpheresByIndices(3, new int[] { 6 });

        // Uniones para esfera 4
        ConnectSpheresByIndices(4, new int[] { 5 });

        // Uniones para esfera 7
        ConnectSpheresByIndices(7, new int[] { 8, 9 });

        // Uniones para esfera 8
        ConnectSpheresByIndices(8, new int[] { 7, 10 });

        // Uniones para esfera 9
        ConnectSpheresByIndices(9, new int[] { 12 });

        // Uniones para esfera 10
        ConnectSpheresByIndices(10, new int[] { 11 });
    }

    void ConnectSpheresByIndices(int startIndex, int[] indices)
    {
        for (int i = 0; i < indices.Length; i++)
        {
            int endIndex = indices[i];

            if (startIndex >= 0 && startIndex < spheres.Count && endIndex >= 0 && endIndex < spheres.Count)
            {
                GameObject startSphere = spheres[startIndex];
                GameObject endSphere = spheres[endIndex];

                Vector3 startPosition = startSphere.transform.position;
                Vector3 endPosition = endSphere.transform.position;

                GameObject line = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
                LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
                lineRenderer.SetPosition(0, startPosition);
                lineRenderer.SetPosition(1, endPosition);

                lines.Add(line);
            }
        }
    }

    void ClearSpheresAndLines()
    {
        foreach (GameObject sphere in spheres)
        {
            Destroy(sphere);
        }
        spheres.Clear();

        foreach (GameObject line in lines)
        {
            Destroy(line);
        }
        lines.Clear();
    }
}
