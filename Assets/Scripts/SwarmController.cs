using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GGJ2024
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SwarmController : MonoBehaviour
    {
        public float FillerSize
        {
            get => fillerSize;
            set
            {
                fillerSize = value;
                RefreshVertices();
            }
        }
        
        public float Spacing
        {
            get => spacing;
            set
            {
                spacing = value;
                RefreshVertices();
            }
        }

        [SerializeField] bool isShowSprite;
        [SerializeField] float fadeInDelay = 2f;
        [SerializeField] float fadeInDuration = 1f;
        [SerializeField] float stayDuration = 1f;
        [SerializeField] float fadeOutDuration = 1f;
        [SerializeField] float fillerSize = 0.1f;
        [SerializeField] float spacing = 0.1f;
        
        public float spread = 1f;
        public Vector2 randomMagnitude;
        
        float currentDelay;
        List<Vector3> vertexList = new List<Vector3>();
        List<Ant> movementList = new List<Ant>();
        SpriteRenderer spriteRenderer;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.DOFade(0, 0);
            RefreshVertices();
        }

        void OnValidate()
        {
            if (!Application.isPlaying)
                return;
            
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            RefreshVertices();
        }

        void RefreshVertices()
        {
            if (fillerSize == 0)
                return;

            if (spacing == 0)
                return;
            
            vertexList.Clear();
            var sprite = spriteRenderer.sprite;
            var texture = sprite.texture;
            var worldBounds = spriteRenderer.bounds;
            var pixelRect = sprite.rect;

            // Multiply by this factor to convert world space size to pixels
            var fillerSizeFactor = Vector2.one / worldBounds.size * pixelRect.size;
            var fillerSizeInPixels = Vector2Int.RoundToInt(fillerSize * fillerSizeFactor);

            var start = worldBounds.min;
            var end = worldBounds.max;

            // Use proper for loops instead of ugly and error prone while ;)
            for (var worldY = start.y; worldY < end.y; worldY += fillerSize + spacing)
            {
                // convert the worldY to pixel coordinate
                var pixelY =
                    Mathf.RoundToInt((worldY - worldBounds.center.y + worldBounds.size.y / 2f) * fillerSizeFactor.y);

                // quick safety check if this fits into the texture pixel space
                if (pixelY + fillerSizeInPixels.y >= texture.height)
                {
                    continue;
                }

                for (var worldX = start.x; worldX < end.x; worldX += fillerSize + spacing)
                {
                    // convert worldX to pixel coordinate
                    var pixelX = Mathf.RoundToInt((worldX - worldBounds.center.x + worldBounds.size.x / 2f) *
                                                  fillerSizeFactor.x);

                    // again the check if this fits into the texture pixel space
                    if (pixelX + fillerSizeInPixels.x >= texture.width)
                    {
                        continue;
                    }

                    // Cut out a rectangle from the texture at given pixel coordinates
                    var pixels = texture.GetPixels(pixelX, pixelY, fillerSizeInPixels.x, fillerSizeInPixels.y);

                    // Using Linq to check if all pixels are transparent
                    if (pixels.All(p => Mathf.Approximately(p.a, 0f)))
                    {
                        continue;
                    }

                    // otherwise spawn a filler here
                    var worldPosition = new Vector3(worldX, worldY, 0);
                    vertexList.Add(transform.InverseTransformPoint(worldPosition));
                }
            }
        }

        void Update()
        {
            var vertexCount = vertexList.Count;
            var index = 0;
            foreach (var movement in movementList)
            {
                if (index >= vertexCount)
                    index = 0;

                var targetPos = transform.TransformPoint(vertexList[index] * spread) + GetRandomNoise();
                movement.SetMovementTarget(new PositionTarget(targetPos), true);
                index++;
            }
        }

        Vector3 GetRandomNoise()
        {
            return new Vector2(Random.Range(-randomMagnitude.x, randomMagnitude.x),
                Random.Range(-randomMagnitude.y, randomMagnitude.y));
        }

        public void Register(Ant[] ant)
        {
            if (ant.IsNullOrEmpty())
                return;
            
            movementList.AddRange(ant);
            if (isShowSprite)
            {
                spriteRenderer.DOKill();
                var seq = DOTween.Sequence();
                seq.AppendInterval(fadeInDelay);
                seq.Append(spriteRenderer.DOFade(1f, fadeInDuration));
                seq.AppendInterval(stayDuration);
                seq.Append(spriteRenderer.DOFade(0f, fadeOutDuration));
                seq.SetTarget(spriteRenderer);
            }
           
        }

        public void Unregister(Ant ant)
        {
            movementList.Remove(ant);
        }

        public Ant[] Release()
        {
            var ants = movementList.ToArray();
            movementList.Clear();
            return ants;
        }
    }
}