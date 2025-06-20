using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent <int, int> healthChanged;
    Animator anm;

    [SerializeField]
    private int _maxHealth = 100;
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    private int _health = 100;
    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            healthChanged?.Invoke(_health, MaxHealth);
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }
    [SerializeField]
    private bool _isAlive = true;
    [SerializeField]
    private bool isInvincible = false; //invincible : baats kha chien baij


    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            anm.SetBool(AnimationStrings.isAlive, value);

            if (value == false)
            {
                damageableDeath.Invoke();
                // Trigger death animation
            }
        }
    }
    // Modify the LockVelocity property to include a private setter so it can be assigned internally.
    public bool LockVelocity
    {
        get
        {
            return anm.GetBool(AnimationStrings.lockVelocity);
        }
        private set
        {
            anm.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    private void Awake()
    {
        anm = GetComponent<Animator>();
    }
    private void Update()
    {
        if (isInvincible)
        {
            if(timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
    }
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            // Nếu anm null thì không gọi SetTrigger
            if (anm != null)
            {
                anm.SetTrigger(AnimationStrings.hitTrigger);
                LockVelocity = true;
            }
            else
            {
                Debug.LogWarning("Animator is missing on " + gameObject.name);
            }

            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;
        }
        return false;
    }

    public void Heal(int healRestore)
    {
        if (IsAlive)
        {
            int maxHealth = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHealth, healRestore);
            Health += actualHeal;

            CharacterEvents.characterHealed.Invoke(gameObject, actualHeal);
        }
    }
    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (anm != null)
        {
            anm.SetTrigger("hit"); // 🔥 Kích hoạt animation bị đánh
        }
    }

}

