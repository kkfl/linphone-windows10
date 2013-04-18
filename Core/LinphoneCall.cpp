#include "LinphoneCall.h"
#include "LinphoneAddress.h"
#include "LinphoneCallStats.h"
#include "LinphoneCallLog.h"
#include "LinphoneCallParams.h"
#include "Server.h"
#include "ApiLock.h"
#include "LinphoneCoreFactory.h"
#include "Globals.h"

using namespace Windows::Phone::Networking::Voip;

Linphone::Core::LinphoneCallState Linphone::Core::LinphoneCall::GetState()
{
	return (Linphone::Core::LinphoneCallState)linphone_call_get_state(this->call);
}

Linphone::Core::LinphoneAddress^ Linphone::Core::LinphoneCall::GetRemoteAddress()
{
	const ::LinphoneAddress *addr = linphone_call_get_remote_address(this->call);
	Linphone::Core::LinphoneAddress^ address = (Linphone::Core::LinphoneAddress^)Linphone::Core::Utils::CreateLinphoneAddress((void*)addr);
	return address;
}

Linphone::Core::CallDirection Linphone::Core::LinphoneCall::GetDirection()
{
	return (Linphone::Core::CallDirection)linphone_call_get_dir(this->call);
}

Linphone::Core::LinphoneCallLog^ Linphone::Core::LinphoneCall::GetCallLog()
{
	return (Linphone::Core::LinphoneCallLog^) Linphone::Core::Utils::CreateLinphoneCallLog(linphone_call_get_call_log(this->call));
}

Linphone::Core::LinphoneCallStats^ Linphone::Core::LinphoneCall::GetAudioStats()
{
	return (Linphone::Core::LinphoneCallStats^) Linphone::Core::Utils::CreateLinphoneCallStats((void*) linphone_call_get_audio_stats(this->call), this->call);
}

Linphone::Core::LinphoneCallParams^ Linphone::Core::LinphoneCall::GetRemoteParams()
{
	if (linphone_call_get_remote_params(this->call) == nullptr)
		return nullptr;

	return (Linphone::Core::LinphoneCallParams^) Linphone::Core::Utils::CreateLinphoneCallParams(linphone_call_params_copy(linphone_call_get_remote_params(this->call)));
}

Linphone::Core::LinphoneCallParams^ Linphone::Core::LinphoneCall::GetCurrentParamsCopy()
{
	return (Linphone::Core::LinphoneCallParams^) Linphone::Core::Utils::CreateLinphoneCallParams(linphone_call_params_copy(linphone_call_get_current_params(this->call)));
}

void Linphone::Core::LinphoneCall::EnableEchoCancellation(Platform::Boolean enable)
{
	std::lock_guard<std::recursive_mutex> lock(g_apiLock);
	linphone_call_enable_echo_cancellation(this->call, enable);
}

Platform::Boolean Linphone::Core::LinphoneCall::IsEchoCancellationEnabled()
{
	return linphone_call_echo_cancellation_enabled(this->call);
}

void Linphone::Core::LinphoneCall::EnableEchoLimiter(Platform::Boolean enable)
{
	std::lock_guard<std::recursive_mutex> lock(g_apiLock);
	linphone_call_enable_echo_limiter(this->call, enable);
}

Platform::Boolean Linphone::Core::LinphoneCall::IsEchoLimiterEnabled()
{
	return linphone_call_echo_limiter_enabled(this->call);
}

int Linphone::Core::LinphoneCall::GetDuration()
{
	return linphone_call_get_duration(this->call);;
}

float Linphone::Core::LinphoneCall::GetCurrentQuality()
{
	return linphone_call_get_current_quality(this->call);
}

float Linphone::Core::LinphoneCall::GetAverageQuality()
{
	return linphone_call_get_average_quality(this->call);
}

Platform::String^ Linphone::Core::LinphoneCall::GetAuthenticationToken()
{
	return Linphone::Core::Utils::cctops(linphone_call_get_authentication_token(this->call));
}

Platform::Boolean Linphone::Core::LinphoneCall::IsAuthenticationTokenVerified()
{
	return linphone_call_get_authentication_token_verified(this->call);
}

void Linphone::Core::LinphoneCall::SetAuthenticationTokenVerified(Platform::Boolean verified)
{
	std::lock_guard<std::recursive_mutex> lock(g_apiLock);
	linphone_call_set_authentication_token_verified(this->call, verified);
}

Platform::Boolean Linphone::Core::LinphoneCall::IsInConference()
{
	return linphone_call_is_in_conference(this->call);
}

float Linphone::Core::LinphoneCall::GetPlayVolume()
{
	return linphone_call_get_play_volume(this->call);
}

Platform::String^ Linphone::Core::LinphoneCall::GetRemoteUserAgent()
{
	return Linphone::Core::Utils::cctops(linphone_call_get_remote_user_agent(this->call));
}

Platform::String^ Linphone::Core::LinphoneCall::GetRemoteContact()
{
	return Linphone::Core::Utils::cctops(linphone_call_get_remote_contact(this->call));
}

void Linphone::Core::LinphoneCall::CallContext::set(Platform::Object^ cc)
{
	this->callContext = cc;
}

Platform::Object^ Linphone::Core::LinphoneCall::GetCallStartTimeFromContext()
{
	if (this->callContext != nullptr) {
		return ((Windows::Phone::Networking::Voip::VoipPhoneCall^)this->callContext)->StartTime;
	}
	return nullptr;
}

Platform::Object^  Linphone::Core::LinphoneCall::CallContext::get()
{
	return this->callContext;
}

Linphone::Core::LinphoneCall::LinphoneCall(::LinphoneCall *call) :
	call(call)
{
	RefToPtrProxy<LinphoneCall^> *proxy = new RefToPtrProxy<LinphoneCall^>(this);
	linphone_call_set_user_pointer(this->call, proxy);
}

Linphone::Core::LinphoneCall::~LinphoneCall()
{
	RefToPtrProxy<LinphoneCall^> *proxy = reinterpret_cast< RefToPtrProxy<LinphoneCall^> *>(linphone_call_get_user_pointer(this->call));
	delete proxy;
}
