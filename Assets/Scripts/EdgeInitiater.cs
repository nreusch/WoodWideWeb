using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EdgeInitiater : MonoBehaviour
{
    private int W = 0;
    private int P = 0;
    private int N = 0;
    private int K = 0;
    [SerializeField] TextMeshProUGUI m_W;
    [SerializeField] TextMeshProUGUI m_P;
    [SerializeField] TextMeshProUGUI m_N;
    [SerializeField] TextMeshProUGUI m_K;

    public void ResourceInitDone(){
        Game game = this.GetComponent<Game>();
        game.finishMakingConnections(W, N, P, K);
    }

    public void WaterOnClickUp(){
		W += 1;
        m_W.text = W.ToString();
	}

    public void WaterOnClickDown(){
        if(W < 1) return;
		W -= 1;
        m_W.text = W.ToString();
	}

    public void PUp(){
		P += 1;
        m_P.text = P.ToString();
	}

    public void PDown(){
        if(P < 1) return;
		P -= 1;
        m_P.text = P.ToString();
	}

    public void NUp(){
		N += 1;
        m_N.text = N.ToString();
	}

    public void NDown(){
        if(N < 1) return;
		N -= 1;
        m_N.text = N.ToString();
	}

    public void KUp(){
		K += 1;
        m_K.text = K.ToString();
	}

    public void KDown(){
        if(K < 1) return;
		K -= 1;
        m_K.text = K.ToString();
	}

    
}
