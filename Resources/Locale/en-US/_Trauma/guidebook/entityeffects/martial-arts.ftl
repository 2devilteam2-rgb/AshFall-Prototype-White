# SPDX-License-Identifier: AGPL-3.0-or-later

entity-condition-guidebook-moving = the target is moving at least { $speed } m/s
entity-condition-guidebook-standing = the target is standing

entity-effect-guidebook-set-standing = {$chance ->
    [1] makes
    *[other] make
} the target {$standing ->
    [true] stand up
    *[other] get knocked down
}

reagent-effect-guidebook-deal-stamina-damage =
    { $chance ->
        [1] { $deltasign ->
                [1] Deals
                *[-1] Heals
            }
        *[other]
            { $deltasign ->
                [1] deal
                *[-1] heal
            }
    } { $amount } { $immediate ->
                    [true] immediate
                    *[false] overtime
                  } stamina damage

reagent-effect-guidebook-drop-items =
    { $chance ->
        [1] Forces
        *[other] force
    } to drop held items

reagent-effect-guidebook-has-status-effect =
    { $invert ->
        [true] has no
        *[false] has
    } {$effect} status effect

reagent-effect-condition-guidebook-stamina-damage-threshold =
    { $max ->
        [2147483648] the target has at least {NATURALFIXED($min, 2)} stamina damage
        *[other] { $min ->
                    [0] the target has at most {NATURALFIXED($max, 2)} stamina damage
                    *[other] the target has between {NATURALFIXED($min, 2)} and {NATURALFIXED($max, 2)} stamina damage
                 }
    }
