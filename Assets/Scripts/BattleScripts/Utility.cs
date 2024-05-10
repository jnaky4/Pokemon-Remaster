using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Pokemon
{
    public class Utility : MonoBehaviour
    {
        public static int randomSeed = 1000;
        public static System.Random rnd = new(randomSeed);
        //public static System.Random rnd = new System.Random();


        /// <summary>
        /// Determines if a hit is a critical or not.
        /// </summary>
        /// <param name="unit">The unit that we are checking for. We need this to access the .pokemon, then the .critical_stage to determine how likely a crit is.</param>
        /// <returns>Returns true if the hit becomes a crit, false otherwise.</returns>
        public static bool CriticalHit(Pokemon pokemon)
        {


            // Next Range is from:
            //      (min to max-1) 
            // ie   1 to (101 - 1) 
            // or   1 to 100

            /*
             * using Gen 2-5 Crit Pobability based on stages
             * Gen II-V	            Gen VI	        Gen VII onwards:
                +0	1/16 (6.25%)	1/16 (6.25%)	1/24 (~4.167%)
                +1	1/8 (12.5%)	    1/8 (12.5%)	    1/8 (12.5%)
                +2	1/4 (25%)	    1/2 (50%)	    1/2 (50%)
                +3	1/3 (~33.3%)	Always (100%)	Always (100%)
                ++4 1/2 (50%)       Always (100%)	Always (100%)
             */
            int num = rnd.Next(1, 101);
            if (pokemon.stage_critical == 0)
            {
                if (num <= 6) return true;
            }
            if (pokemon.stage_critical == 1)
            {
                if (num <= 13) return true;
            }
            if (pokemon.stage_critical == 2)
            {
                if (num <= 25) return true;
            }
            if (pokemon.stage_critical == 3)
            {
                if (num <= 33) return true;
            }
            if (pokemon.stage_critical == 4)
            {
                if (num <= 50) return true;
            }
            return false;
        }

        //todo validate this is accurate
        public static double CritChance(Pokemon poke)
        {
            double chance = 0;
            switch (poke.stage_critical)
            {
                case 0:
                    chance = 1.0 / 16.0;
                    break;
                case 1:
                    chance = 1.0 / 8.0;
                    break;
                case 2:
                    chance = 1.0 / 4.0;
                    break;
                case 3:
                    chance = 1.0 / 3.0;
                    break;
                case 4:
                case 5:
                case 6:
                    chance = 1.0 / 2.0;
                    break;
            }
            return chance;

        }


        /// <summary>
        /// Calculates how much damage is done based on the types of the attacker and defender
        /// /// Using Gen 3 Damage Calculations
        /// <param name="attacker">The attacker unit.</param>
        /// <param name="defender">The defender unit.</param>
        /// <param name="attack">The move we want to use.</param>
        /// <param name="crit">A bool that lets us know if the attack is a crit or not.</param>
        /// <returns>This returns the only damage calculation of the attack</returns>
//         public static int CalculateDamage(Unit attacker, Unit defender, Moves attack, bool crit, double dmg_multiplier, double stab, double rand = -1)
//         {

//             double damage = 0;
//             //calculates the damage multiplier for attacking move on both defender types
//             //double dmg_multiplier = EffectivenessMultiplier(attack, defender.pokemon);
//             double critical = crit ? 1.5 : 1; //If it is a crit, multiply by 1.5

//             //From Generation III onward, it is a random integer percentage between 0.85 and 1.00 (inclusive)
//             double random = rand;

//             double attacker_burn_multiplier = attacker.pokemon.HasStatus("Burn") ? .5 : 1;

//             if (random != -1)
//             {
//                 //assume inccorect range set, should be 101
//                 if (random == 100) random += 1;
//                 random /= 100;
//             }
//             else
//             {
//                 //85 to 100.

//                 double num = rnd.Next(85, 101);
//                 random = num / 100; //Random number for the random element.
//                 /*Debug.Log("Random: " + random);*/
//             }
            


//             //doesnt get used
// /*            if (attacker.pokemon.statuses.Contains(Status.get_status("Burn")))
//             {
//                 burn = Status.get_status("Burn").affect_stat_mulitplier;
//             }*/

//             if (attack.base_power > 0)
//             {
//                 if(attack.category == "Physical")
//                 {
//                     /*                    
//                     Debug.Log("Physical Attack");
//                     Debug.Log(attacker.pokemon.name);
//                     Debug.Log("current attack: " + attacker.pokemon.current_attack);
//                     Debug.Log("atk stage: " + attacker.pokemon.stage_attack);
//                     Debug.Log("attacker Burned: " + attacker.pokemon.HasStatus("Burn"));

//                     Debug.Log(defender.pokemon.name);
//                     Debug.Log("current def: " + defender.pokemon.current_defense);
//                     Debug.Log("def stage: " + defender.pokemon.stage_defense);
//                     */
//                     double attack_vs_defense = attacker.pokemon.current_attack / defender.pokemon.current_defense;
//                     attack_vs_defense = attack_vs_defense == 0 ? 1 : attack_vs_defense;


                        
//                     damage = (((((2 * attacker.pokemon.level) / 5) + 2) * attack.base_power * (attack_vs_defense)) / 50) + 2; //Basic attacking
//                     damage *= attacker_burn_multiplier; //Burn only applies to physical
//                 }
//                 else
//                 {
//                     /*                    
//                     Debug.Log("Special Attack");
//                     Debug.Log(attacker.pokemon.name);
//                     Debug.Log("current sp_attack: " + attacker.pokemon.current_sp_attack);
//                     Debug.Log("sp_atk stage: " + attacker.pokemon.stage_sp_attack);

//                     Debug.Log(defender.pokemon.name);
//                     Debug.Log("current sp_defense: " + defender.pokemon.current_sp_defense);
//                     Debug.Log("sp_def stage: " + defender.pokemon.stage_sp_defense);

//                     Debug.Log(defender.pokemon.name + " current Sp_def: " + defender.pokemon.current_sp_defense);
//                     */

//                     double sp_attack_vs_sp_defense = attacker.pokemon.current_sp_attack / defender.pokemon.current_sp_defense;
//                     sp_attack_vs_sp_defense = sp_attack_vs_sp_defense == 0 ? 1 : sp_attack_vs_sp_defense;

//                     damage = (((((2 * attacker.pokemon.level) / 5) + 2) * attack.base_power * (sp_attack_vs_sp_defense)) / 50) + 2;
//                     //Debug.Log("Attacker Sp_atk: " + attacker.pokemon.current_sp_attack);
//                     //Debug.Log("Defender Sp_def: " + defender.pokemon.current_sp_defense);

//                     //Debug.Log("Damage = ((2 * (level)" + attacker.pokemon.level + " * (base power)" + attack.base_power + " * (sp_atk/sp_def)" + (attacker.pokemon.current_sp_attack / defender.pokemon.current_sp_defense) + ") / 50) + 2 * (crit)" + critical + " * (stab)" + stab + " * rand " + rand + " * (multplier)" + dmg_multiplier);
//                 }
                
//                 damage *= (critical * stab * random * dmg_multiplier); //Extra multipliers.
//                 //immune moves will never deal damage
//                 if (dmg_multiplier == 0) damage = 0;
//                 //damaging moves always do 1 damage
//                 else if (damage == 0) damage = 1;
//             }
//             else
//             {
//                 damage = 0;
//             }
//             if (damage < 0) damage = 0;

//             attacker.damage = damage;

//             return (int)damage;
//         }
       public static int CalculateDamage(Pokemon attacker, Pokemon defender, Moves attack)
        {
            //A/D Attack/Defense || Special Attack/Special Defense 

            //TODO targets, weather, FF Flash Fire,
            double ad = 1, burn = 1, screen = 1, targets = 1, weather = 1, ff = 1;
            //TODO HH Helping Hand, Stockpile, DoubleDmg, Charge
            double crits = 1, HH = 1, STAB = 1, types = 1, r =1;

            switch(attack.category){
                case "Physical": 
                    ad = (attacker.max_attack * StageMultiplier("Attack", attacker)) / (defender.max_defense * StageMultiplier("Defense", defender));
                    //Reflect effects apply to all Pokémon on the user's side of the field
                    screen = defender.HasStatus("Reflect") ? .5 : 1;
                    //halves the damage a burned Pokémon does with physical moves
                    burn = attacker.HasStatus("Burn") ? .5 : 1;
                    break;
                case "Special":
                    ad = (attacker.max_sp_attack * StageMultiplier("Special Attack", attacker)) / ( defender.max_sp_defense * StageMultiplier("Special Defense", defender));
                    //Light Screen effects apply to all Pokémon on the user's side of the field
                    screen = defender.HasStatus("Light Screen")  ? .5 : 1;
                    break;
            }

            //using globals for dialogue
            BattleSystem.crit = CriticalHit(attacker);
            crits = BattleSystem.crit ? 1.5 : 1;

            STAB = Utility.STAB(attack, attacker);

            types = EffectivenessMultiplier(attack, defender);
            BattleSystem.effectivenessMultiplier = types;

            r = .85 + (rnd.NextDouble() * (1.0 - .85));




            /// ( ((2 x level / 5) + 2) x Power x A/D ) / 50
            /// x Burn x Screen x Targets x Weather x FF + 2 ) 
            /// x Stockpile x Critical x DoubleDmg x Charge x HH x STAB x Type1 x Type2 x Random
            /// Ref: https://bulbapedia.bulbagarden.net/wiki/Damage
            
            
            double newDamage = (((2 * attacker.level / 5 * attack.base_power * ad / 50) + 2) * burn * screen * targets * weather * ff + 2) * crits * HH * STAB * types * r;
            // newDamage *= ad * burn * screen * targets * weather * ff + 2;
            // newDamage *= crits * HH * STAB * types * r;

            //example lvl 75 glaceon (atk stat 123) ice fang (65 physical) vs garchomp (163 def) -> 4x weakness, no crit
            // (2 * 75 / 5 + 2) x 65 x 123/163 / 50 + 2) * 1 * 1 * 1 * 1 * rand * 1.5 * 4 * 1 * 1
            newDamage = newDamage > 0 ? newDamage : 0;
            return (int)newDamage;

        }
        public static double StageAccuracyEvasionMultiplier(string stage, Pokemon Poke){
            double stageLevel, multiplier;

            switch(stage){
                case "Accuracy":
                    stageLevel = Poke.stage_accuracy;
                    break;
                case "Evasion":
                    stageLevel = Poke.stage_evasion;
                    break;
                default:
                    Debug.Log("stage " + stage + "not found");
                    stageLevel = 0;
                    break;
            }

            switch (stageLevel){
                case -6:
                    multiplier = .33;
                    break;
                case -5:
                    multiplier = .36;
                    break;
                case -4:
                    multiplier = .43;
                    break;
                case -3:
                    multiplier = .5;
                    break;
                case -2:
                    multiplier = .6;
                    break;
                case -1:
                    multiplier = .75;
                    break;
                case 0:
                    multiplier = 1;
                    break;
                case 1:
                    multiplier = 1.33;
                    break;
                case 2:
                    multiplier = 1.66;
                    break;
                case 3:
                    multiplier = 2;
                    break;
                case 4:
                    multiplier = 2.5;
                    break;
                case 5:
                    multiplier = 2.66;
                    break;
                case 6:
                    multiplier = 3;
                    break;
                default:
                    Debug.Log("Non a valid stage value:" + stageLevel);
                    multiplier = 1;
                    break;
            }
            return multiplier;
        }
        
        public static double StageMultiplier(string stage, Pokemon Poke){
            double stageLevel, multiplier;

            switch(stage){
                case "Attack":
                    stageLevel = Poke.stage_attack;
                    break;
                case "Defense":
                    stageLevel = Poke.stage_defense;
                    break;
                case "Special Attack":
                    stageLevel = Poke.stage_sp_attack;
                    break;
                case "Special Defense":
                    stageLevel = Poke.stage_sp_defense;
                    break;
                case "Speed":
                    stageLevel = Poke.stage_speed;
                    break;
                case "Accuracy":
                    return StageAccuracyEvasionMultiplier(stage, Poke);
                case "Critical":
                    stageLevel = Poke.stage_critical;
                    break;
                case "Evasion":
                    return StageAccuracyEvasionMultiplier(stage, Poke);
                default:
                    Debug.Log("stage " + stage + "not found");
                    stageLevel = 0;
                    break;
            }

            switch (stageLevel){
                case -6:
                    multiplier = .25;
                    break;
                case -5:
                    multiplier = .285;
                    break;
                case -4:
                    multiplier = .333;
                    break;
                case -3:
                    multiplier = .4;
                    break;
                case -2:
                    multiplier = .5;
                    break;
                case -1:
                    multiplier = .666;
                    break;
                case 0:
                    multiplier = 1;
                    break;
                case 1:
                    multiplier = 1.5;
                    break;
                case 2:
                    multiplier = 2;
                    break;
                case 3:
                    multiplier = 2.5;
                    break;
                case 4:
                    multiplier = 3;
                    break;
                case 5:
                    multiplier = 3.5;
                    break;
                case 6:
                    multiplier = 4;
                    break;
                default:
                    Debug.Log("Non a valid stage value:" + stageLevel);
                    multiplier = 1;
                    break;
            }

            return multiplier;

        }
        public static double EffectivenessMultiplier(Moves attack, Pokemon defender)
        {
            return Type.attacking_type_dict[attack.type.name][defender.type1.name] * Type.attacking_type_dict[attack.type.name][defender.type2.name];
        }

        /// <summary>
        /// Determines if the enemy Pokemon has any remaining moves or if it has to use struggle.
        /// </summary>
        /// <returns>True if they have to struggle, false otherwise.</returns>
        public static bool EnemyStruggle(Unit unit)
        {
            int i;
            bool struggle = false;

            for (i = 0; i < unit.pokemon.currentMoves.Count(s => s != null); i++)
            {
                if (unit.pokemon.currentMoves[i].current_pp != 0)
                {
                    struggle = false;
                    break;
                }
                struggle = true;
            }
            return struggle;
        }

        public static double STAB(Moves attack, Pokemon attacker)
        {
            return attack.type.name == attacker.type1.name || attack.type.name == attacker.type2.name ? 1.5 : 1;
        }

        public static BattleState WhoGoesFirst(Moves playerMove, Moves enemyMove, Pokemon playerPokemon, Pokemon enemyPokemon)
        {

            //priorities arent the same
            //1,0 or 0,1
            if (playerMove.priority != enemyMove.priority)
            {
                return playerMove.priority > enemyMove.priority ? BattleState.PLAYERTURN : BattleState.ENEMYTURN;

            }

            double playerPokemonSpeed = playerPokemon.max_speed * StageMultiplier("Speed", playerPokemon);
            double enemyPokemonSpeed = enemyPokemon.max_speed * StageMultiplier("Speed", enemyPokemon);
            //priorities are the same, do additional checks
            //1,1, or 0,0
            if(playerPokemonSpeed != enemyPokemonSpeed)
            {
                double player_paralyzed_multiplier = playerPokemon.HasStatus("Paralysis") ? .5 : 1;
                double enemy_paralyzed_multiplier = enemyPokemon.HasStatus("Paralysis") ? .5 : 1; 

                return playerPokemonSpeed * player_paralyzed_multiplier > enemyPokemonSpeed * enemy_paralyzed_multiplier ? BattleState.PLAYERTURN : BattleState.ENEMYTURN;
            }
            //same priority, same speed
            else
            {

                return rnd.Next(2) < 1 ? BattleState.PLAYERTURN : BattleState.ENEMYTURN;
            }

        }

        public static bool IsLethal(int damage, Pokemon target) => damage >= target.current_hp;


        //TODO add status effects to turns until faint
        public static int TurnsUntilFaint(int attackerDamage, Pokemon defending, BattleState whoGoesFirst)
        {
            int turnsUntilFaint = -1;
            double self_damage = getStatusDamage(defending);
            int remaining_hp = defending.current_hp;



            if (attackerDamage > 0)
            {
                turnsUntilFaint = 0;
                while(remaining_hp > 0)
                {
                    //Debug.Log("BURN DMG: " + Status.get_status("Burn").self_damage);
                    //Debug.Log("BURN DMG: " + self_damage);
                    //Debug.Log("Defender HP: " + remaining);
                    remaining_hp -= attackerDamage;
                    //Debug.Log("Attacker Dmg: " + attackerDamage);
                    remaining_hp -= (int)(defending.max_hp * self_damage);
                    //Debug.Log("Burn Dmg: " + (int)(defending.max_hp * self_damage));
                    turnsUntilFaint++;

                }

                //turnsUntilFaint = (int)Math.Ceiling((double)defending.current_hp / (double)attackerDamage);

                //turnsUntilFaint += whoGoesFirst == BattleState.PLAYERTURN ? 0 : 1;
            }
            return turnsUntilFaint;
        }

        public static Dictionary<string, int> CalculateEachMoveDamage(Pokemon attacking_pokemon, Unit Attacker, Unit Defender)
        {
            Dictionary<string, int> available_moves = new();

            for (int i = 0; i < attacking_pokemon.CountMoves(); i++)
            {
                //Debug.Log(this.pokemon.currentMoves[i].name);

                if (attacking_pokemon.currentMoves[i].base_power > 0 && attacking_pokemon.currentMoves[i].current_pp > 0)
                {
                    double emul = EffectivenessMultiplier(attacking_pokemon.currentMoves[i], Defender.pokemon);
                    double stab = STAB(attacking_pokemon.currentMoves[i], attacking_pokemon);
                    //Debug.Log("Effectiveness: " + emul);
                    int potentialDmg = CalculateDamage(Attacker.pokemon, Defender.pokemon, attacking_pokemon.currentMoves[i]);
                    available_moves.Add(attacking_pokemon.currentMoves[i].name, potentialDmg);
                    //Debug.Log(attacking_pokemon.currentMoves[i].name + " might do " + potentialDmg + " damage.");
                }
            }
            return available_moves;
        }


        /// <summary>
        /// Takes a Dictionary of Pokemon Moves and their estimated damage 
        /// and returns the best attack based on damage. If damage is equal between 2 moves, 
        /// checks if one has higher priority
        /// </summary>
        /// <param name="available_moves"></param>
        /// <returns>the best attack based on damage or null if no move was chosen</returns>
        public static Moves DecideHighestDamagingAttack(Dictionary<string, int> available_moves)
        {
            int decided_move_damage = 0;
            Moves decided_move = null;

            foreach (KeyValuePair<string, int> attack in available_moves)
            {
                //If attack does more damage replace decided move
                if (attack.Value > decided_move_damage)
                {
                    decided_move_damage = attack.Value;
                    decided_move = Moves.get_move(attack.Key);
                }

                else if (attack.Value == decided_move_damage)
                {
                    //if moves do same damage and new one has higher priority, replace decided move
                    if (Moves.get_move(attack.Key).priority > decided_move.priority)
                    {
                        decided_move_damage = attack.Value;
                        decided_move = Moves.get_move(attack.Key);

                    }
                    //TODO should we add other checks like status chance?
                    //else if ()
                }
            }

            return decided_move;
        }

/*        public static Dictionary<string, int> CalculateEachMoveStatus(Pokemon pokemon, Unit Defender)
        {
            Dictionary<string, int> available_moves = new Dictionary<string, int>();

            for (int i = 0; i < pokemon.CountMoves(); i++)
            {
                //Debug.Log(this.pokemon.currentMoves[i].name);

*//*                if (pokemon.currentMoves[i].base_power > 0 && pokemon.currentMoves[i].current_pp > 0)
                {
                    double emul = Utility.EffectivenessMultiplier(pokemon.currentMoves[i], Defender.pokemon);
                    double stab = Utility.STAB(pokemon.currentMoves[i], pokemon);
                    //Debug.Log("Effectiveness: " + emul);
                    int potentialDmg = Utility.CalculateDamage(Attacker, Defender, pokemon.currentMoves[i], false, emul, stab);
                    available_moves.Add(pokemon.currentMoves[i].name, potentialDmg);
                    Debug.Log(pokemon.currentMoves[i].name + " might do " + potentialDmg + " damage.");
                }*//*
            }
            return available_moves;
        }*/

        public static bool AMIFaster(Pokemon I, Pokemon Enemy)
        {
            return I.max_speed > Enemy.max_speed;
        }

        public static Moves DecideBestStatusAttack(Pokemon attackingPokemon, Pokemon defendingPokemon, int turnsUnilThisFaints = 0, int TurnsUntilEnemyFaints = 0, bool EnemyAttackLethal = false)
        {
            
            Moves decided_move = null;
            double decided_move_status_chance = 0;

            for (int i = 0; i < attackingPokemon.CountMoves(); i++)
            {
                //does move have status
                if(attackingPokemon.currentMoves[i].status.name != "null")
                {
                    //is status persisting and defending already has
                    if (attackingPokemon.currentMoves[i].status.persistence && defendingPokemon.HasPersistenceStatus())
                    {
                        Debug.Log(defendingPokemon.name + " already has a persisting status, can't apply another");
                    }
                    //is defending immune to status
                    else if (defendingPokemon.ImmuneToStatus(attackingPokemon.currentMoves[i].status))
                    {
                        Debug.Log(defendingPokemon.name + " is immune to " + attackingPokemon.currentMoves[i].status.name);
                    }
                    //compare status with decided move status
                    else
                    {
                        Status decided_move_status;
                        //is there a move yet?
                        if (decided_move == null)
                        {
                            decided_move = attackingPokemon.currentMoves[i];
                            decided_move_status = attackingPokemon.currentMoves[i].status;
                            decided_move_status_chance = attackingPokemon.currentMoves[i].status_chance;
                        }

                        else
                        {
                            if (decided_move_status_chance < attackingPokemon.currentMoves[i].status_chance)
                            {
                                decided_move = attackingPokemon.currentMoves[i];
                                decided_move_status = attackingPokemon.currentMoves[i].status;
                                decided_move_status_chance = attackingPokemon.currentMoves[i].status_chance;
                            }


                            /*                            if (EnemyAttackLethal)
                                                        {
                                                            if (Utility.AMIFaster(attackingPokemon, defendingPokemon))
                                                            {

                                                            }


                                                        }*/
                            //check which status is best
                            //Which move has the highest chance of status
                            //Which status is best Freeze > Sleep > Paralysis > Burn > poison : confusion, Flinch, Seeded?
                            //

                        }


                    }
                 
                }
            }

            return decided_move;
        }
        

        public static double getStatusDamage(Pokemon pokemon)
        {
            double status_damage = 0;

            if (pokemon.HasStatus("Burn"))
            {
                status_damage = Status.GetStatus("Burn").self_damage;
            }
            if (pokemon.HasStatus("Poison"))
            {
                status_damage = Status.GetStatus("Poison").self_damage;
            }
            if (pokemon.HasStatus("Confusion"))
            {
                status_damage = Status.GetStatus("Confusion").self_damage;
            }
            if (pokemon.HasStatus("Seeded"))
            {
                status_damage = Status.GetStatus("Seeded").self_damage;
            }

            return status_damage;
        }
        public static Moves LeastDamagingMoveThatKills(Dictionary<string, int> available_moves, Pokemon Target)
        {

            KeyValuePair<string, int> leastDamagingLethal = new("null", 0);
            foreach (KeyValuePair<string, int> attack in available_moves)
            {
                if (Utility.IsLethal(attack.Value, Target) && attack.Value > leastDamagingLethal.Value)
                {
                    leastDamagingLethal = attack;
                }
            }
            if (leastDamagingLethal.Value > 0) return Moves.get_move(leastDamagingLethal.Key);
            else return null;
        }
    }
}

