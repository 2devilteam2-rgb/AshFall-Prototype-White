# SPDX-License-Identifier: AGPL-3.0-or-later

entity-condition-guidebook-moving = цель движется со скоростью не менее { $speed } м/с
entity-condition-guidebook-standing = цель стоит на ногах

entity-effect-guidebook-set-standing = {$chance ->
    [1] заставляет
    *[other] заставить
} цель {$standing ->
    [true] встать
    *[other] упасть
}

reagent-effect-guidebook-deal-stamina-damage =
    { $chance ->
        [1] { $deltasign ->
                [1] Наносит
                *[-1] Восстанавливает
            }
        *[other]
            { $deltasign ->
                [1] наносит
                *[-1] восстанавливает
            }
    } { $amount } единиц урона по выносливости { $immediate ->
                    [true] мгновенно
                    *[false] постепенно
                  }

reagent-effect-guidebook-drop-items =
    { $chance ->
        [1] Заставляет
        *[other] заставить
    } выронить предметы из рук

reagent-effect-guidebook-has-status-effect =
    { $invert ->
        [true] не имеет
        *[false] имеет
    } эффект состояния {$effect}

reagent-effect-condition-guidebook-stamina-damage-threshold =
    { $max ->
        [2147483648] цель получила урона по выносливости не менее {NATURALFIXED($min, 2)}
        *[other] { $min ->
                    [0] цель получила урона по выносливости не более {NATURALFIXED($max, 2)}
                    *[other] цель получила урона по выносливости от {NATURALFIXED($min, 2)} до {NATURALFIXED($max, 2)}
                 }
    }
