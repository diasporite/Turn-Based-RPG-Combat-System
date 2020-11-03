using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG_Project
{
    public class FloatingText : MonoBehaviour
    {
        Text floatingText;

        [SerializeField] float lifetime = 0.5f;
        [SerializeField] float speed = 1f;
        [SerializeField] Vector3 dir = new Vector3(0, 1);

        public Text _floatingText => floatingText;

        public float _lifetime
        {
            get => lifetime;
            set => lifetime = value;
        }

        public float _speed
        {
            get => speed;
            set => speed = value;
        }

        public Vector3 _dir
        {
            get => dir;
            set => dir = value;
        }

        public void SetText(string message)
        {
            if (floatingText == null) floatingText = GetComponentInChildren<Text>();

            floatingText.text = message;
        }

        public void SetTextColour(Color colour)
        {
            floatingText.color = colour;
        }

        public void SetOutlineColour(Color colour)
        {
            floatingText.GetComponent<Outline>().effectColor = colour;
        }

        public IEnumerator TextFloatCo(string message, Color textColour, Color outlineColour)
        {
            float t = 0;

            SetText(message);
            SetTextColour(textColour);
            SetOutlineColour(outlineColour);

            while (t < lifetime)
            {
                t += Time.deltaTime;
                transform.position += speed * dir * Time.deltaTime;
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}