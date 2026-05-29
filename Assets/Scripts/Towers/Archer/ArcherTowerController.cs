using System.Collections;
using UnityEngine;

public class ArcherTowerController : MonoBehaviour
{

    [Header("Data Tower")]
    [SerializeField] private CastleData _archerDataTower;

    [SerializeField] private float _timeInitTower = 1.5f;
    [SerializeField] private ArcherTowerSetupAnimation _archerTowerSetupAnimation;
    [SerializeField] private HeroArcherAnimation _hero1ArcherAnimation;
    [SerializeField] private HeroArcherAnimation _hero2ArcherAnimation;

    private void Start()
    {
        StartCoroutine(CoroutineCreateTower());
    }

    private IEnumerator CoroutineCreateTower()
    {
        yield return new WaitForSeconds(_timeInitTower);
        _archerTowerSetupAnimation.InitTower(_archerDataTower.animationTower, 
            _archerDataTower.frameTowerStartIdle, _archerDataTower.frameTowerEndIdle);
        SetupHeroPosition();
        //_hero1ArcherAnimation.Idle(new Vector2(1,0));
        //_hero2ArcherAnimation.Idle(new Vector2(1, 0));
    }

    private void SetupHeroPosition()
    {
        _hero1ArcherAnimation.transform.localPosition = _archerDataTower.heroPositionList[0];
        _hero2ArcherAnimation.transform.localPosition = _archerDataTower.heroPositionList[1];
    }

    public CastleData GetDataTower()
    {
        return _archerDataTower;
    }
}
