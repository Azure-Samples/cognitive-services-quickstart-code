# <snippet_1>
from azure.cognitiveservices.personalizer import PersonalizerClient
from azure.cognitiveservices.personalizer.models import RankableAction, RewardRequest, RankRequest
from msrest.authentication import CognitiveServicesCredentials

import datetime, json, os, time, uuid, random

key = "paste_your_personalizer_key_here"
endpoint = "paste_your_personalizer_endpoint_here"

# Instantiate a Personalizer client
client = PersonalizerClient(endpoint, CognitiveServicesCredentials(key))

actions_and_features = {
    'pasta': {
        'brand_info': {
            'company':'pasta_inc'
        }, 
        'attributes': {
            'qty':1, 'cuisine':'italian',
            'price':12
        },
        'dietary_attributes': {
            'vegan': False,
            'low_carb': False,
            'high_protein': False,
            'vegetarian': False,
            'low_fat': True,
            'low_sodium': True
        }
    },
    'bbq': {
        'brand_info' : {
            'company': 'ambisco'
        },
        'attributes': {
            'qty': 2,
            'category': 'bbq',
            'price': 20
        }, 
        'dietary_attributes': {
            'vegan': False,
            'low_carb': True,
            'high_protein': True,
            'vegetarian': False,
            'low_fat': False,
            'low_sodium': False
        }
    },
    'bao': {
        'brand_info': {
            'company': 'bao_and_co'
        },
        'attributes': {
            'qty': 4,
            'category': 'chinese',
            'price': 8
        }, 
        'dietary_attributes': {
            'vegan': False,
            'low_carb': True,
            'high_protein': True,
            'vegetarian': False,
            'low_fat': True,
            'low_sodium': False
        }
    },
    'hummus': {
        'brand_info' : { 
            'company': 'garbanzo_inc'
        },
        'attributes' : {
            'qty': 1,
            'category': 'breakfast',
            'price': 5
        }, 
        'dietary_attributes': {
            'vegan': True, 
            'low_carb': False,
            'high_protein': True,
            'vegetarian': True,
            'low_fat': False, 
            'low_sodium': False
        }
    },
    'veg_platter': {
        'brand_info': {
            'company': 'farm_fresh'
        }, 
        'attributes': {
            'qty': 1,
            'category': 'produce', 
            'price': 7
        },
        'dietary_attributes': {
            'vegan': True,
            'low_carb': True,
            'high_protein': False,
            'vegetarian': True,
            'low_fat': True,
            'low_sodium': True
        }
    }
}

def get_actions():
    res = []
    for action_id, feat in actions_and_features.items():
        action = RankableAction(id=action_id, features=[feat])
        res.append(action)
    return res

user_profiles = {
    'Bill': {
        'dietary_preferences': 'low_carb', 
        'avg_order_price': '0-20',
        'browser_type': 'edge'
    },
    'Satya': {
        'dietary_preferences': 'low_sodium',
        'avg_order_price': '201+',
        'browser_type': 'safari'
    },
    'Amy': {
        'dietary_preferences': {
            'vegan', 'vegetarian'
        },
        'avg_order_price': '21-50',
        'browser_type': 'edge'},
    }

def get_context(user):
    location_context = {'location': random.choice(['west', 'east', 'midwest'])}
    time_of_day = {'time_of_day': random.choice(['morning', 'afternoon', 'evening'])}
    app_type = {'application_type': random.choice(['edge', 'safari', 'edge_mobile', 'mobile_app'])}
    res = [user_profiles[user], location_context, time_of_day, app_type]
    return res

def get_random_users(k = 5):
    return random.choices(list(user_profiles.keys()), k=k)


def get_reward_score(user, actionid, context):
    reward_score = 0.0
    action = actions_and_features[actionid]
    
    if user == 'Bill':
        if action['attributes']['price'] < 10 and (context[1]['location'] !=  "midwest"):
            reward_score = 1.0
            print("Bill likes to be economical when he's not in the midwest visiting his friend Warren. He bought", actionid, "because it was below a price of $10.")
        elif (action['dietary_attributes']['low_carb'] == True) and (context[1]['location'] ==  "midwest"):
            reward_score = 1.0
            print("Bill is visiting his friend Warren in the midwest. There he's willing to spend more on food as long as it's low carb, so Bill bought" + actionid + ".")
            
        elif (action['attributes']['price'] >= 10) and (context[1]['location'] != "midwest"):
            print("Bill didn't buy", actionid, "because the price was too high when not visting his friend Warren in the midwest.")
            
        elif (action['dietary_attributes']['low_carb'] == False) and (context[1]['location'] ==  "midwest"):
            print("Bill didn't buy", actionid, "because it's not low-carb, and he's in the midwest visitng his friend Warren.")
             
    elif user == 'Satya':
        if action['dietary_attributes']['low_sodium'] == True:
            reward_score = 1.0
            print("Satya is health conscious, so he bought", actionid,"since it's low in sodium.")
        else:
            print("Satya did not buy", actionid, "because it's not low sodium.")   
            
    elif user == 'Amy':
        if (action['dietary_attributes']['vegan'] == True) or (action['dietary_attributes']['vegetarian'] == True):
            reward_score = 1.0
            print("Amy likes to eat plant-based foods, so she bought", actionid, "because it's vegan or vegetarian friendly.")       
        else:
            print("Amy did not buy", actionid, "because it's not vegan or vegetarian.")
                
    return reward_score
    # ...
# </snippet_1>
# <snippet_2>
def run_personalizer_cycle():
    actions = get_actions()
    user_list = get_random_users()
    for user in user_list:
        print("------------")
        print("User:", user, "\n")
        context = get_context(user)
        print("Context:", context, "\n")
        
        rank_request = RankRequest(actions=actions, context_features=context)
        response = client.rank(rank_request=rank_request)
        print("Rank API response:", response, "\n")
        
        eventid = response.event_id
        actionid = response.reward_action_id
        print("Personalizer recommended action", actionid, "and it was shown as the featured product.\n")
        
        reward_score = get_reward_score(user, actionid, context)
        client.events.reward(event_id=eventid, value=reward_score)     
        print("\nA reward score of", reward_score , "was sent to Personalizer.")
        print("------------\n")

continue_loop = True
while continue_loop:
    run_personalizer_cycle()
    
    br = input("Press Q to exit, or any other key to run another loop: ")
    if(br.lower()=='q'):
        continue_loop = False
# </snippet_2>

# <snippet_multi>
for i in range(0,1000):
    run_personalizer_cycle()
# </snippet_multi>
