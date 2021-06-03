import datetime, json, os, time, uuid, requests

# The endpoint specific to your personalization service instance;
# e.g. https://<your-resource-name>.cognitiveservices.azure.com
PERSONALIZATION_BASE_URL = "PASTE_YOUR_PERSONALIZER_ENDPOINT_HERE"
# The key specific to your personalization service instance; e.g. "0123456789abcdef0123456789ABCDEF"
RESOURCE_KEY = "PASTE_YOUR_PERSONALIZER_SUBSCRIPTION_KEY_HERE"
MULTI_SLOT_RANK_URL = '{0}personalizer/v1.1-preview.1/multislot/rank'.format(PERSONALIZATION_BASE_URL)
MULTI_SLOT_REWARD_URL_BASE = '{0}personalizer/v1.1-preview.1/multislot/events/'.format(PERSONALIZATION_BASE_URL) #add "{eventId}/reward"
HEADERS = {
    'ocp-apim-subscription-key': RESOURCE_KEY,
    'Content-Type': 'application/json'
}

def get_actions():
    return [
        {
            "id": "Red-Polo-Shirt-432",
            "features": [
                {
                    "onSale": "true",
                    "price": 20,
                    "category": "Clothing"
                }
            ]
        },
        {
            "id": "Tennis-Racket-133",
            "features": [
                {
                    "onSale": "false",
                    "price": 70,
                    "category": "Sports"
                }
            ]
        },
        {
            "id": "31-Inch-Monitor-771",
            "features": [
                {
                    "onSale": "true",
                    "price": 200,
                    "category": "Electronics"
                }
            ]
        },
        {
            "id": "XBox-Series X-117",
            "features": [
                {
                    "onSale": "false",
                    "price": 499,
                    "category": "Electronics"
                }
            ]
        }
    ]

def get_slots():
    return [
        {
            "id": "BigHeroPosition",
            "features": [
                {
                    "size": "large",
                    "position": "left",
                }
            ],
            "excludedActions": ["31-Inch-Monitor-771"],
            "baselineAction": "Red-Polo-Shirt-432"
        },
        {
            "id": "SmallSidebar",
            "features": [
                {
                    "size": "small",
                    "position": "right",
                }
            ],
            "excludedActions": ["Tennis-Racket-133"],
            "baselineAction": "XBox-Series X-117"
        }
    ]

def get_context_features():
    time_features = ["morning", "afternoon", "evening", "night"]
    time_pref = input("What time of day is it (enter number)? 1. morning 2. afternoon 3. evening 4. night\n")
    try:
        parsed_time = int(time_pref)
        if(parsed_time <=0 or parsed_time > len(time_features)):
            raise IndexError
        time_of_day = time_features[parsed_time-1]
    except (ValueError, IndexError):
        print("Entered value is invalid. Setting feature value to", time_features[0] + ".")
        time_of_day = time_features[0]

    device_features = ['mobile', 'tablet', 'desktop']
    device_pref = input("What type of device is the user on (enter number)? 1. mobile 2. tablet 3. desktop\n")
    try:
        parsed_device = int(device_pref)
        if(parsed_device <=0 or parsed_device > len(device_features)):
            raise IndexError
        device = device_features[parsed_device-1]
    except (ValueError, IndexError):
        print("Entered value is invalid. Setting feature value to", device_features[0]+ ".")
        device = device_features[0]

    return [
        {'time': time_of_day},
        {'device': device}
        ]

def send_multi_slot_rank(rank_request):
    multi_slot_response = requests.post(MULTI_SLOT_RANK_URL, data=json.dumps(rank_request), headers=HEADERS )
    return json.loads(multi_slot_response.text)

def get_reward_for_slot():
    answer = input('\nIs this correct? (y/n)\n').upper()
    if (answer == 'Y'):
        print('\nGreat! The application will send Personalizer a reward of 1 so it learns from this choice of action for this slot.\n')
        return 1
    elif (answer == 'N'):
        print('\nYou didn\'t like the recommended item.The application will send Personalizer a reward of 0 for this choice of action for this slot.\n')
        return 0
    print('\nEntered choice is invalid. Service assumes that you didn\'t like the recommended item.\n')
    return 0

def send_multi_slot_reward(reward_request, event_id):
    reward_url = '{0}{1}/reward'.format(MULTI_SLOT_REWARD_URL_BASE, event_id)
    requests.post(reward_url, data=json.dumps(reward_request), headers=HEADERS)

run_loop = True

while run_loop:

    eventId = str(uuid.uuid4())
    context = get_context_features()
    actions = get_actions()
    slots = get_slots()

    rank_request = {
        "eventId": eventId,
        "contextFeatures": context,
        "actions": actions,
        "slots": slots,
        "deferActivation": False
      }

    #Rank the actions for each slot
    multi_slot_rank_response = send_multi_slot_rank(rank_request)
    multi_slot_rewards = {"reward": []}

    for i in range(len(multi_slot_rank_response['slots'])):
        print('\nPersonalizer service decided you should display: {0} in slot {1}\n'.format(multi_slot_rank_response['slots'][i]['rewardActionId'], multi_slot_rank_response['slots'][i]['id']))

        slot_reward = {'slotId': multi_slot_rank_response['slots'][i]['id']}
        # User agrees or disagrees with Personalizer decision for slot
        slot_reward['value'] = get_reward_for_slot()
        multi_slot_rewards['reward'].append(slot_reward)

    # Send the rewards for the event
    send_multi_slot_reward(multi_slot_rewards, multi_slot_rank_response['eventId'])

    answer = input('\nPress q to break, any other key to continue:\n').upper()
    if (answer == 'Q'):
        run_loop = False
