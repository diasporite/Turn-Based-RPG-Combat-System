using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG_Project
{
    // Allows the battler sprite to be manipulated on certain moves
    [RequireComponent(typeof(SpriteRenderer))]
    public class BattlePortrait : MonoBehaviour
    {
        [SerializeField] CharID id;

        [SerializeField] BattleChar character;

        Sprite sprite;

        SpriteRenderer sr;

        public CharID _id => id;

        public BattleChar _character { get { return character; }
            set { character = value; } }

        public void InitPortrait(BattleChar character)
        {
            this.character = character;

            sr = GetComponent<SpriteRenderer>();

            sr.sprite = character._sprite;
        }

        public void Die()
        {
            StartCoroutine(DieCo(0.5f));
        }

        public IEnumerator ShakeCo(float dx, float dt)
        {
            float t = 0;
            float w = 8f * Mathf.PI;
            Vector3 startPos = transform.position;
            Vector3 dir = 0.25f * dx * Mathf.Sign(transform.position.x) * Vector3.right;

            while (t < dt)
            {
                t += Time.deltaTime;
                transform.position = startPos + Mathf.Sin(w * t) * dir;
                yield return null;
            }

            transform.position = startPos;
        }

        public IEnumerator DieCo(float dt)
        {
            yield return StartCoroutine(FadeTo(0, 0, 0, 1, 0.5f * dt));

            yield return StartCoroutine(FadeAlpha(0, 0.5f * dt));
        }

        IEnumerator FadeTo(float r, float g, float b, float a, float dt)
        {
            float t = 0;

            var r0 = sr.color.r;
            var g0 = sr.color.g;
            var b0 = sr.color.b;
            var a0 = sr.color.a;

            var dr = r - sr.color.r;
            var dg = g - sr.color.g;
            var db = b - sr.color.b;
            var da = a - sr.color.a;

            while (t < 1)
            {
                t += Time.deltaTime / dt;

                sr.color = new Color(r0 + t * dr, g0 + t * dg, b0 + t * db, a0 + t * da);

                yield return null;
            }

            sr.color = new Color(r, g, b, a);
        }

        IEnumerator FadeAlpha(float a, float dt)
        {
            float t = 0;

            var r = sr.color.r;
            var g = sr.color.g;
            var b = sr.color.b;

            var a0 = sr.color.a;

            var da = a - sr.color.a;

            while (t < 1)
            {
                t += Time.deltaTime / dt;

                sr.color = new Color(r, g, b, a0 + t * da);

                yield return null;
            }

            sr.color = new Color(r, g, b, a);
        }
    }
}