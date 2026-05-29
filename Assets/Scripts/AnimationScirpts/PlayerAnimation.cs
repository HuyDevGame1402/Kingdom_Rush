using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public SpriteSheetAnimator animator;
    public GameObject targetObject; // Kéo object có SpriteRenderer vào đây

    public GameObject basePlatformObj;
    public GameObject soldier1Obj;
    public GameObject soldier2Obj;
    public GameObject cannonObj;
    public GameObject muzzleFlashObj;
    public GameObject bombSfxObj;

    void Update()
    {
        //AnimationP3();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.PlayAnimation(targetObject, "tower_artillery_lvl1_layer6_", 29,29);
        }
    }

    private void AnimationP1()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "arcane_tower_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "artillery_lvl4_tesla_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "arcanehit_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "artillery_lvl4_bfg_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "archer_tower_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "arcane_teleport_effect_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "arcane_shooter_");
        }
    }

    private void AnimationP2()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "atomicBomb_plane_engine_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "atomicBomb_plane_wing_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "bleeding_big_gray_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "bleeding_big_green_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "bleeding_big_orange_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "bleeding_big_red_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "bleeding_big_violet_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "bombs_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "build_terrain_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "burn_big_");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "curse_big_");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "curse_boss_type1_");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "decal_smoke_hitground_");
        }

        // 
        if (Input.GetKeyDown(KeyCode.D))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "effect_buildSmoke_");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "effect_sellSmoke_");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "elfSoldier_");
        }
    }

    private void AnimationP3()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "elfTower_layer1_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "explosion_air_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "explosion_big_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "explosion_fragment_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "explosion_shrapnel_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "fireball_explosion_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "fireball_particle_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "fireball_proyectile_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "freeze_creepFlying_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "freeze_creep_");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "fx_blood_splat_gray_");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "fx_blood_splat_red_");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "fx_blood_splat_green_");
        }

        // 
        if (Input.GetKeyDown(KeyCode.D))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "fx_blood_splat_violet_");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "fx_blood_splat_orange_");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "fx_bullet_smoke_");
        }
    }

    private void AnimationP4()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "fx_coin_jump_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "fx_polymorph_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "fx_rifle_smoke_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "fx_smoke_hitground_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "healing_big_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "healing_boss_type1_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "healing_small_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            // Chạy animation Tesla
            //animator.PlayAnimation(targetObject, "mage_lvl1_");
            animator.PlayAnimation(gameObject,
            "mage_lvl1_", 2, 12);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "mage_lvl2_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "mage_lvl3_");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "mage_shooter_");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "magebolt_");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "particle_sniper_bullet_");
        }

        // 
        if (Input.GetKeyDown(KeyCode.D))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "poison_big_");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "hero_archer_arrow_");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "hero_artillery_brea_shot");
        }
    }

    private void AnimationP6()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "pop_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "ray_arcane_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "ray_desintegrate_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "ray_polymorph_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "ray_tesla_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "reinforce_A0_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "reinforce_A1_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "reinforce_A2_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "reinforce_A3_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "reinforce_B0_");
        }
    }
    private void AnimationP7()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "pop_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "ray_arcane_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "ray_desintegrate_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "ray_polymorph_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "ray_tesla_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "reinforce_A0_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "reinforce_A1_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "reinforce_A2_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "reinforce_A3_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "reinforce_B0_");
        }
    }
    private void AnimationP89()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "small_freeze_bomb");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "small_freeze_explosion_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "soldier_elemental_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "soldier_lvl1_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "soldier_lvl2_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "soldier_lvl3_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "soldier_lvl4_barbarian_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "soldier_lvl4_paladin_");
        }
    }
    private void AnimationP10()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "sorcerer_shooter_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "sorcerer_tower_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "sorcererbolt_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "states_big_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "states_small_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "static_particle_");
        }
    }
    private void AnimationP11()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "sorcerer_shooter_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "sorcerer_tower_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "sorcererbolt_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "states_big_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "states_small_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "static_particle_");
        }
    }
    private void AnimationP12()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "stun_big_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "stun_small_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "sunray_Rays_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "teslahit_big_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "teslahit_boss_type1_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "teslahit_small_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "thor_hammer_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "thorn_big_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "thorn_small_");
        }
    }
    private void AnimationP13()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "tower_archer_druid_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "tower_archer_lvl1_shooter_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_archer_lvl2_shooter_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "tower_archer_lvl3_shooter_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "tower_archer_musketeer_shooter_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_archer_ranger_shooter_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_artillery_lvl1_layer1_");
        }
    }
    private void AnimationP14()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "tower_artillery_lvl1_layer2_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "tower_artillery_lvl1_layer3_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_artillery_lvl1_layer4_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "tower_artillery_lvl1_layer5_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "tower_artillery_lvl1_layer6_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_artillery_lvl1_layer7_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_artillery_lvl2_layer1_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "tower_artillery_lvl2_layer2_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_artillery_lvl2_layer3_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_artillery_lvl2_layer4_");
        }
    }
    private void AnimationP15()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "tower_artillery_lvl2_layer6_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "tower_artillery_lvl2_layer7_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_artillery_lvl3_layer1_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "tower_artillery_lvl3_layer2_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "tower_artillery_lvl3_layer3_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_artillery_lvl3_layer4_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_artillery_lvl3_layer5_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "tower_artillery_lvl3_layer6_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_artillery_lvl3_layer7_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_barrack_lvl4_Barbarians_layer1_");
        }
    }
    private void AnimationP16()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "tower_barrack_lvl4_Barbarians_layer2_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "tower_barracks_lvl1_layer1_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_barracks_lvl1_layer2_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Chạy animation tháp Arcane
            animator.PlayAnimation(targetObject, "tower_barracks_lvl2_layer1_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "tower_barracks_lvl2_layer2_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_barracks_lvl3_layer1_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_barracks_lvl3_layer2_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "tower_barracks_lvl4_Paladins_layer1_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_barracks_lvl4_Paladins_layer2_");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_constructing_");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Chạy animation Tesla
            animator.PlayAnimation(targetObject, "tower_preview_archer");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_preview_artillery");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_preview_barrack");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            // Hiệu ứng nổ Arcane Hit
            animator.PlayAnimation(targetObject, "tower_preview_mage");
        }
    }
}
