using UnityEngine;

public class AudioManagerTester : MonoBehaviour
{
    [Header("Test Settings")]
    [SerializeField] private bool showInstructions = true;

    private void Update()
    {
        // Müzik Kontrolleri
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AudioManager.Instance.PlayMusic(0);
            Debug.Log("Müzik 1 çalıyor");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AudioManager.Instance.PlayMusic(1);
            Debug.Log("Müzik 2 çalıyor");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AudioManager.Instance.PlayMusic(2);
            Debug.Log("Müzik 3 çalıyor");
        }

        // Müzik Fade ile Geçiş
        if (Input.GetKeyDown(KeyCode.F))
        {
            int randomMusic = Random.Range(0, 4);
            AudioManager.Instance.PlayMusicWithFade(randomMusic, 2f);
            Debug.Log($"Müzik {randomMusic + 1} fade ile çalıyor");
        }

        // Müzik Durdur/Duraklat/Devam
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (AudioManager.Instance.IsMusicPlaying())
            {
                AudioManager.Instance.PauseMusic();
                Debug.Log("Müzik duraklatıldı");
            }
            else
            {
                AudioManager.Instance.ResumeMusic();
                Debug.Log("Müzik devam ediyor");
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            AudioManager.Instance.StopMusic();
            Debug.Log("Müzik durduruldu");
        }

        // SFX Kontrolleri
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AudioManager.Instance.PlaySFX(0);
            Debug.Log("SFX 1 çalındı");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            AudioManager.Instance.PlaySFX(1);
            Debug.Log("SFX 2 çalındı");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.Instance.PlaySFX(2);
            Debug.Log("SFX 3 çalındı");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            AudioManager.Instance.PlaySFX(3);
            Debug.Log("SFX 4 çalındı");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            AudioManager.Instance.PlaySFX(4);
            Debug.Log("SFX 5 çalındı");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            AudioManager.Instance.PlaySFX(5);
            Debug.Log("SFX 6 çalındı");
        }

        // Çoklu SFX Testi (aynı anda birden fazla ses)
        if (Input.GetKeyDown(KeyCode.X))
        {
            for (int i = 0; i < 5; i++)
            {
                AudioManager.Instance.PlaySFX(i);
            }
            Debug.Log("5 rastgele SFX aynı anda çalındı!");
        }

        // Volume Kontrolleri
        if (Input.GetKey(KeyCode.UpArrow))
        {
            AudioManager.Instance.SetMusicVolume(
                Mathf.Clamp01(AudioManager.Instance.GetComponent<AudioSource>().volume + Time.deltaTime * 0.5f)
            );
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            AudioManager.Instance.SetMusicVolume(
                Mathf.Clamp01(AudioManager.Instance.GetComponent<AudioSource>().volume - Time.deltaTime * 0.5f)
            );
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            float currentVolume = AudioManager.Instance.GetComponent<AudioSource>().volume;
            AudioManager.Instance.SetSFXVolume(
                Mathf.Clamp01(currentVolume + Time.deltaTime * 0.5f)
            );
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            float currentVolume = AudioManager.Instance.GetComponent<AudioSource>().volume;
            AudioManager.Instance.SetSFXVolume(
                Mathf.Clamp01(currentVolume - Time.deltaTime * 0.5f)
            );
        }

        // Mute Toggle
        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioManager.Instance.ToggleMute();
            Debug.Log("Mute toggle");
        }

        // Tüm SFX'leri Durdur
        if (Input.GetKeyDown(KeyCode.C))
        {
            AudioManager.Instance.StopAllSFX();
            Debug.Log("Tüm SFX'ler durduruldu");
        }

        // Yardım Menüsü
        if (Input.GetKeyDown(KeyCode.H))
        {
            showInstructions = !showInstructions;
        }
    }

    private void OnGUI()
    {
        if (!showInstructions) return;

        GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
        boxStyle.fontSize = 14;
        boxStyle.alignment = TextAnchor.UpperLeft;
        boxStyle.padding = new RectOffset(10, 10, 10, 10);

        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = 12;
        labelStyle.normal.textColor = Color.white;

        // Arka plan kutusu
        GUI.Box(new Rect(10, 10, 350, 480), "🎵 Audio Manager Test Kontrolleri", boxStyle);

        int yPos = 40;
        int lineHeight = 22;

        // Müzik Kontrolleri
        GUI.Label(new Rect(20, yPos, 330, 20), "=== MÜZİK KONTROLLER ===", labelStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(20, yPos, 330, 20), "1, 2, 3, 4: Müzik 1-4 çal", labelStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(20, yPos, 330, 20), "F: Rastgele müzik fade ile çal", labelStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(20, yPos, 330, 20), "Space: Müzik Duraklat/Devam", labelStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(20, yPos, 330, 20), "S: Müzik Durdur", labelStyle);
        yPos += lineHeight + 10;

        // SFX Kontrolleri
        GUI.Label(new Rect(20, yPos, 330, 20), "=== SFX KONTROLLER ===", labelStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(20, yPos, 330, 20), "Q, W, E, R, T: SFX 1-5 çal", labelStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(20, yPos, 330, 20), "X: 5 rastgele SFX aynı anda çal", labelStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(20, yPos, 330, 20), "C: Tüm SFX'leri durdur", labelStyle);
        yPos += lineHeight + 10;

        // Volume Kontrolleri
        GUI.Label(new Rect(20, yPos, 330, 20), "=== VOLUME KONTROLLER ===", labelStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(20, yPos, 330, 20), "↑ / ↓: Müzik volume artır/azalt", labelStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(20, yPos, 330, 20), "→ / ←: SFX volume artır/azalt", labelStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(20, yPos, 330, 20), "M: Mute/Unmute", labelStyle);
        yPos += lineHeight + 10;

        // Diğer
        GUI.Label(new Rect(20, yPos, 330, 20), "=== DİĞER ===", labelStyle);
        yPos += lineHeight;
        GUI.Label(new Rect(20, yPos, 330, 20), "H: Bu menüyü göster/gizle", labelStyle);
        yPos += lineHeight + 20;

        // Durum Bilgisi
        GUI.Label(new Rect(20, yPos, 330, 20), "=== DURUM ===", labelStyle);
        yPos += lineHeight;
        
        if (AudioManager.Instance != null)
        {
            bool isPlaying = AudioManager.Instance.IsMusicPlaying();
            int currentMusic = AudioManager.Instance.GetCurrentMusicIndex();
            
            GUI.Label(new Rect(20, yPos, 330, 20), 
                $"Müzik Durumu: {(isPlaying ? "▶ Çalıyor" : "⏸ Duraklatıldı/Durduruldu")}", 
                labelStyle);
            yPos += lineHeight;
            GUI.Label(new Rect(20, yPos, 330, 20), 
                $"Aktif Müzik: {currentMusic + 1}", 
                labelStyle);
        }
        else
        {
            GUI.Label(new Rect(20, yPos, 330, 20), 
                "❌ AudioManager bulunamadı!", 
                labelStyle);
        }
    }
}