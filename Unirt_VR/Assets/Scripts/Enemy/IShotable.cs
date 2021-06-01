using UnityEngine;

// 총과 인터렉션이 있는 모든 타입이 공통적으로 가져야 하는 인터페이스
public interface IShotAble{
    // 총과 인터렉션이 있는 모든 타입들은 IShotAble를 상속하고 OnShot 메서드를 반드시 구현해야 한다
    void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal);
}