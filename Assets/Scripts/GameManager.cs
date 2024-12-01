using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro.EditorUtilities;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using TMPro;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    class SaveData{
        public string nickname;
        public int highestScore;
    }

    public static GameManager Instance;

    // Player Information
    public  TMP_InputField nicknameHolder;
    public string nickname;

    // Highest Score
    public string highestScorer;
    public int highestScore;
    public TMP_Text highestScore_text;

    
    void Awake(){
    
        LoadHighestScore();
        if(Instance != null){
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveHighestScore(){

        SaveData data = new SaveData(){
            nickname = nickname,
            highestScore = highestScore
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/highest_score.json",json);
    }   

    public void LoadHighestScore(){
        string path = Application.persistentDataPath + "/highest_score.json";
        
        if(File.Exists(path)){
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highestScorer = data.nickname;
            highestScore = data.highestScore;
            highestScore_text.text = ($"Best Score : {highestScorer} | {highestScore} ");
        }
    }

    public void StartNew(){
        nickname = nicknameHolder.text;
        SceneManager.LoadScene(1);
    }

    public void Exit(){
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #else
        Application.Quit(); // original code to quit Unity player
        #endif  
    }
}
